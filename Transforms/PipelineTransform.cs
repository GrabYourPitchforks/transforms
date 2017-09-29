namespace System
{
    public class PipelineTransform<TIn, TOut> : ITransform<TIn, TOut>
    {
        private readonly ITransform<TIn, TOut> _transform;
        public PipelineTransform(ITransform<TIn, TOut> transform)
        {
            _transform = transform ?? throw new ArgumentNullException(nameof(transform));
        }

        public virtual bool CanTransformInPlace =>
            _transform.CanTransformInPlace;

        public PipelineTransform<TIn, byte> Append(ITransform<TOut, byte> transform) =>
            Append<byte>(transform);

        public PipelineTransform<TIn, char> Append(ITransform<TOut, char> transform) =>
            Append<char>(transform);

        public PipelineTransform<TIn, Utf8Char> Append(ITransform<TOut, Utf8Char> transform) =>
            Append<Utf8Char>(transform);

        public virtual PipelineTransform<TIn, TOutNew> Append<TOutNew>(ITransform<TOut, TOutNew> transform)
        {
            // can also optimize this code path based on CanTransformInPlace
            throw new NotImplementedException("Magic happens here.");
        }

        public virtual int GetMaxNumberOutputElements(int inputElementCount) =>
            _transform.GetMaxNumberOutputElements(inputElementCount);

        public virtual bool TryGetTransformedElementCount(ReadOnlySpan<TIn> input, bool isFinalChunk, out int numElementsOutput)
            => _transform.TryGetTransformedElementCount(input, isFinalChunk, out numElementsOutput);

        public virtual bool TryTransform(ReadOnlySpan<TIn> input, bool isFinalChunk, Span<TOut> output, out int numElementsRead, out int numElementsWritten)
            => _transform.TryTransform(input, isFinalChunk, output, out numElementsRead, out numElementsWritten);
    }
}
