using System;
using System.Collections.Generic;
using System.Text;

namespace Transforms
{
    public class Utf8Encoder : IStatelessTransform<char, byte>, IStatelessTransform<Utf8Char, byte>
    {
        public bool CanTransformInPlace => throw new NotImplementedException();

        public long GetMaxOutputElementCount(long numInputElements)
        {
            throw new NotImplementedException();
        }

        [return: MustInspect]
        public TransformStatus Transform(ReadOnlySpan<char> input, Span<byte> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten)
        {
            throw new NotImplementedException();
        }

        [return: MustInspect]
        public TransformStatus Transform(ReadOnlySpan<Utf8Char> input, Span<byte> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten)
        {
            throw new NotImplementedException();
        }

        public bool TryGetTransformedElementCount(ReadOnlySpan<char> input, bool isFinalChunk, out long numOutputElements)
        {
            throw new NotImplementedException();
        }

        public bool TryGetTransformedElementCount(ReadOnlySpan<Utf8Char> input, bool isFinalChunk, out long numOutputElements)
        {
            throw new NotImplementedException();
        }
    }
}
