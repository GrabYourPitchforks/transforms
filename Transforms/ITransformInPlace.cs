using System;

namespace Transforms
{
    /// <summary>
    /// Represents an <see cref="ITransform{T, T}"/> that may allow transforming
    /// data in-place within a single buffer.
    /// </summary>
    /// <typeparam name="T">The type of the data to undergo transformation.</typeparam>
    public interface ITransformInPlace<T> : ITransform<T, T>
    {
        /// <summary>
        /// <see langword="true"/> if this transform allows transforming data in-place,
        /// <see langword="false"/> if transforming data in-place is not supported.
        /// </summary>
        bool CanTransformInPlace { get; }

        /// <summary>
        /// Transforms elements within a buffer. The total number of output elements
        /// may be fewer than or greater than the total number of input elements.
        /// </summary>
        /// <param name="data">
        /// A buffer containing the data to be transformed.
        /// The transformation will start from the first element in the buffer.
        /// </param>
        /// <param name="inputLength">The number of elements in <paramref name="data"/> to be transformed.</param>
        /// <param name="outputLength">
        /// If this method returns <see langword="true"/>, contains the number of transformed
        /// elements written to the buffer. The output starts at the first element in the buffer.
        /// If this method returns <see langword="false"/>, undefined.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the input data is well-formed and the buffer is large enough
        /// to hold the final result of transforming all input elements.
        /// <see langword="false"/> if the input data is not well-formed or if the buffer is not
        /// large enough to allow for a total transformation of all input elements.
        /// </returns>
        bool TryTransformInPlace(Span<T> data, int inputLength, out int outputLength);
    }
}
