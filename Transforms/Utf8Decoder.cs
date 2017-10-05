using System;
using System.Collections.Generic;
using System.Text;

namespace Transforms
{
    public class Utf8Decoder : IStatelessTransform<byte, char>, IStatelessTransform<byte, Utf8Char>
    {
        public bool CanTransformInPlace => throw new NotImplementedException();

        public int GetMaxOutputElementCount(int numInputElements)
        {
            throw new NotImplementedException();
        }

        [return: MustInspect]
        public TransformStatus Transform(ReadOnlySpan<byte> input, Span<char> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten)
        {
            throw new NotImplementedException();
        }

        [return: MustInspect]
        public TransformStatus Transform(ReadOnlySpan<byte> input, Span<Utf8Char> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten)
        {
            throw new NotImplementedException();
        }

        public bool TryGetTransformedElementCount(ReadOnlySpan<byte> input, bool isFinalChunk, out int numOutputElements)
        {
            throw new NotImplementedException();
        }
    }
}
