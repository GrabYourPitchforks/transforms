using System;
using System.Collections.Generic;
using System.Text;

namespace Transforms
{
    public interface IStatefulTransform<TIn, TOut> : IDisposable
    {
        bool IsOutputAvailable { get; }

        bool TryFlush(Span<TOut> output, out int numElementsWritten);

        bool TryGetRemainingOutput(Span<TOut> output, out int numElementsWritten, out bool finished);

        bool TryGetRemainingOutputCount(out int availableOutputElements);

        bool TryTransform(ReadOnlySpan<TIn> input, Span<TOut> output, bool isFinalChunk, out int inputElementsConsumed, out int outputElementsWritten);

    }
}
