using System;
using System.Collections.Generic;
using System.Text;

namespace Transforms
{
    /// <summary>
    /// Provides a mechanism for transforming data from one representation to another in
    /// a stateless manner. Data is provided to the <see cref="Transform(ReadOnlySpan{TIn}, Span{TOut}, bool, out int, out int)"/>
    /// method in chunks.
    /// </summary>
    /// <typeparam name="TIn">The source type representing the data to be transformed.</typeparam>
    /// <typeparam name="TOut">The destination type representing the transformed data.</typeparam>
    public interface IStatelessTransform<TIn, TOut>
    {
        /// <summary>
        /// States whether the same buffer can be provided as both the 'input' and the 'output'
        /// parameter in the <see cref="Transform(ReadOnlySpan{TIn}, Span{TOut}, bool, out int, out int)"/> method.
        /// </summary>
        bool CanTransformInPlace { get; }

        /// <summary>
        /// Determines the maximum number of output elements that can result from an input of the given length.
        /// </summary>
        /// <param name="numInputElements">The number of input elements from which to determine the
        /// maximum possible number of output elements.</param>
        /// <returns>The maximum possible number of output elements.</returns>
        /// <remarks>
        /// This method throws an exception if <paramref name="numInputElements"/> is negative or if the maximum
        /// output element count is greater than <see cref="Int32.MaxValue"/>.
        /// </remarks>
        int GetMaxOutputElementCount(int numInputElements);

        /// <summary>
        /// Transforms data from one representation to another.
        /// </summary>
        /// <param name="input">A buffer of input elements which serves as the source of the data to transform.</param>
        /// <param name="output">A buffer of output elements which serves as the destination for the transformed data.</param>
        /// <param name="isFinalChunk"><see langword="true"/> if <paramref name="input"/> contains the last of the data to
        /// transform. <see langword="false"/> if the caller will provide more data in another call.</param>
        /// <param name="numElementsConsumed">When this method returns, contains the number of elements from <paramref name="input"/>
        /// which were consumed during the transformation.</param>
        /// <param name="numElementsWritten">When this method returns, contains the number of elements written to <paramref name="output"/>.</param>
        /// <returns>A <see cref="TransformStatus"/>.</returns>
        /// <remarks>
        /// If <paramref name="isFinalChunk"/> is <see langword="true"/> and this method returns <see cref="TransformStatus.Incomplete"/>,
        /// then <paramref name="output"/> was not large enough to hold the entire result.
        /// </remarks>
        [return: MustInspect]
        TransformStatus Transform(ReadOnlySpan<TIn> input, Span<TOut> output, bool isFinalChunk, out int numElementsConsumed, out int numElementsWritten);

        /// <summary>
        /// Gets the number of output elements which will result from transforming the provided input buffer.
        /// </summary>
        /// <param name="input">A buffer of input elements which serves as the source of the data to transform.</param>
        /// <param name="isFinalChunk"><see langword="true"/> if <paramref name="input"/> contains the last of the data to
        /// transform. <see langword="false"/> if the caller will provide more data in another call.</param>
        /// <param name="numOutputElements">When this method returns, contains the number of elements which results
        /// from transforming the data provided in <paramref name="input"/>.</param>
        /// <returns><see langword="true"/> if the number of output elements was calculated successfully.
        /// <see langword="false"/> if the the data provided by <paramref name="input"/> was invalid or if
        /// the number of output elements could not be successfully calculated.</returns>
        bool TryGetTransformedElementCount(ReadOnlySpan<TIn> input, bool isFinalChunk, out int numOutputElements);
    }
}
