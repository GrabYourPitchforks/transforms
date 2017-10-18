using System;

namespace Transforms
{
    /// <summary>
    /// Provides a mechanism for transforming data from one representation to another in
    /// a stateless manner. Data is provided to the <see cref="TryTransform(ReadOnlySpan{TIn}, Span{TOut}, bool, out int, out int)"/>
    /// method in chunks.
    /// </summary>
    /// <typeparam name="TIn">The source type representing the input data to be transformed.</typeparam>
    /// <typeparam name="TOut">The destination type representing the transformed output data.</typeparam>
    public interface ITransform<TIn, TOut>
    {
        /// <summary>
        /// Determines the maximum number of output elements that can result from transforming an input of the given length.
        /// </summary>
        /// <param name="inputLength">The number of input elements.</param>
        /// <returns>The maximum number of output elements that could result from the transform.</returns>
        /// <remarks>
        /// This method throws an exception if <paramref name="inputLength"/> is negative or if the return
        /// value cannot fit in a <see cref="long"/>.
        /// </remarks>
        long GetMaxOutputLength(long inputLength);

        /// <summary>
        /// Determines the exact size of the buffer required to hold all of the output which would result from a call to
        /// <see cref="TryTransform(ReadOnlySpan{TIn}, Span{TOut}, bool, out int, out int)"/> with the specified input.
        /// </summary>
        /// <param name="input">The source of the data to transform.</param>
        /// <param name="isFinalChunk"><see langword="true"/> if <paramref name="input"/> contains the last of the data to
        /// transform. <see langword="false"/> if the caller will provide more data in another call.</param>
        /// <param name="outputLength">
        /// If this method returns <see langword="true"/>, contains the number of output elements.
        /// If this method returns <see langword="false"/>, undefined.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the input is well-formed and if the exact length of the required output buffer
        /// can be determined. <see langword="false"/> if the input is not well-formed or if the required output buffer
        /// length cannot fit into a <see cref="long"/>.
        /// </returns>
        bool TryGetOutputLength(ReadOnlySpan<TIn> input, bool isFinalChunk, out long outputLength);

        /// <summary>
        /// Transforms elements from a source buffer, placing the transformed elements in a destination buffer.
        /// </summary>
        /// <param name="input">The source of the data to transform.</param>
        /// <param name="output">
        /// The destination buffer in which to place transformed data.
        /// If this method returns <see langword="true"/>, then <paramref name="numWritten"/> elements
        /// were written to the destination buffer. If this method returns <see langword="false"/>,
        /// the contents of this buffer are undefined.
        /// </param>
        /// <param name="isFinalChunk"><see langword="true"/> if <paramref name="input"/> contains the last of the data to
        /// transform. <see langword="false"/> if the caller will provide more data in another call.</param>
        /// <param name="numConsumed">
        /// If this method returns <see langword="true"/>, contains the number of elements from <paramref name="numConsumed"/>
        /// which were successfully transformed. This may be less than the total number of input elements.
        /// If this method returns <see langword="false"/>, undefined.
        /// </param>
        /// <param name="numWritten">
        /// If this method returns <see langword="true"/>, contains the number of elements written to <paramref name="output"/>.
        /// This may be less than the total length of the output buffer.
        /// If this method returns <see langword="false"/>, undefined.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the input data is well-formed and zero or more elements were tranformed
        /// from the source buffer to the destination buffer. <see langword="false"/> if the input data is not
        /// well-formed.
        /// </returns>
        /// <remarks>
        /// It is possible for this method to return <see langword="true"/> even if no data was transformed,
        /// such as in the case where the input buffer or the output buffer is not large enough to allow even
        /// a partial transformation. In this case the caller should provide a larger input or output buffer
        /// and attempt the call again. If this method returns <see langword="false"/>, the input data is not
        /// well-formed and the caller should not attempt the call again with the same input data, as it will
        /// never succeed.
        /// </remarks>
        bool TryTransform(ReadOnlySpan<TIn> input, Span<TOut> output, bool isFinalChunk, out int numConsumed, out int numWritten);
    }
}
