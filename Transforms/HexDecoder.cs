﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Transforms
{
    public class HexDecoder : IStatelessTransform<char, byte>, IStatelessTransform<Utf8Char, byte>
    {
        public bool CanTransformInPlace => throw new NotImplementedException();

        public int GetMaxOutputElementCount(int numInputElements)
        {
            throw new NotImplementedException();
        }

        public bool TryGetTransformedElementCount(ReadOnlySpan<char> input, bool isFinalChunk, out int numOutputElements)
        {
            throw new NotImplementedException();
        }

        public bool TryGetTransformedElementCount(ReadOnlySpan<Utf8Char> input, bool isFinalChunk, out int numOutputElements)
        {
            throw new NotImplementedException();
        }

        public bool TryTransform(ReadOnlySpan<char> input, Span<byte> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten)
        {
            throw new NotImplementedException();
        }

        public bool TryTransform(ReadOnlySpan<Utf8Char> input, Span<byte> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten)
        {
            throw new NotImplementedException();
        }
    }
}