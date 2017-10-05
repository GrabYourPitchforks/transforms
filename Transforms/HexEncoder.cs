using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Numerics;

namespace Transforms
{
    public class HexEncoder : IStatelessTransform<byte, char>, IStatelessTransform<byte, Utf8Char>
    {
        private readonly bool _useLowercase;

        public HexEncoder()
            : this(useLowercase: false)
        {
        }

        public HexEncoder(bool useLowercase)
        {
            _useLowercase = useLowercase;
        }

        public bool CanTransformInPlace { get; } = false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint GetHexValue(uint nibble, uint addendWhenAlpha)
        {
            Debug.Assert(nibble < 16);
            return nibble + ((nibble < 10) ? '0' : addendWhenAlpha);
        }

        public int GetMaxOutputElementCount(int numInputElements) => (int)checked((uint)numInputElements * 2);

        [return: MustInspect]
        public TransformStatus Transform(ReadOnlySpan<byte> input, Span<char> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten)
        {
            return TransformCommon<char>(input, output, isFinalChunk, out numElementsConsumed, out numElementsWritten);
        }

        [return: MustInspect]
        public TransformStatus Transform(ReadOnlySpan<byte> input, Span<Utf8Char> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten)
        {
            return TransformCommon<Utf8Char>(input, output, isFinalChunk, out numElementsConsumed, out numElementsWritten);
        }

        private TransformStatus TransformCommon<TOut>(ReadOnlySpan<byte> input, Span<TOut> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten)
        {
            int elementsToConsumeCount = Math.Min(input.Length, output.Length / 2);

            TransformCore(
                   input: ref input.DangerousGetPinnableReference(),
                   inputLength: elementsToConsumeCount,
                   output: ref Unsafe.Add(ref Unsafe.As<TOut, byte>(ref output.DangerousGetPinnableReference()), (BitConverter.IsLittleEndian) ? 0 : (Unsafe.SizeOf<TOut>() - 1)),
                   outputStride: Unsafe.SizeOf<TOut>(),
                   addendWhenAlpha: (_useLowercase) ? (uint)('a' - 10) : (uint)('A' - 10));

            numElementsConsumed = elementsToConsumeCount;
            numElementsWritten = elementsToConsumeCount * 2;
            return (numElementsConsumed == input.Length) ? TransformStatus.FullyCompleted : TransformStatus.PartiallyCompleted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void TransformCore(ref byte input, int inputLength, ref byte output, int outputStride, uint addendWhenAlpha)
        {
            for (int i = 0; i < inputLength; i++)
            {
                uint thisByte = Unsafe.Add(ref input, i);
                Unsafe.Add(ref output, outputStride * (2 * i)) = (byte)GetHexValue(thisByte >> 4, addendWhenAlpha);
                Unsafe.Add(ref output, outputStride * (2 * i + 1)) = (byte)GetHexValue(thisByte & 0xf, addendWhenAlpha);
            }
        }

        public bool TryGetTransformedElementCount(ReadOnlySpan<byte> input, bool isFinalChunk, out int numOutputElements)
        {
            // We can't report an output element count that overflows Int32.
            // This is ok if this isn't the final chunk, since the caller will just invoke this method in a loop.
            // If this is the final chunk, the caller expects us to give him an exact count, which we can't do.

            long actualOutputCount = input.Length * 2 /* output elements per input element */;
            numOutputElements = (int)Math.Min(actualOutputCount, Int32.MinValue);
            return (!isFinalChunk || (numOutputElements == actualOutputCount));
        }
    }
}
