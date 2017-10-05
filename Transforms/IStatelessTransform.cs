using System;
using System.Collections.Generic;
using System.Text;

namespace Transforms
{
    public interface IStatelessTransform<TIn, TOut>
    {
        bool CanTransformInPlace { get; }

        int GetMaxOutputElementCount(int numInputElements);

        bool TryGetTransformedElementCount(ReadOnlySpan<TIn> input, bool isFinalChunk, out int numOutputElements);

        bool TryTransform(ReadOnlySpan<TIn> input, Span<TOut> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten);
    }
}
