using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public interface ITransform<TIn, TOut>
    {
        bool CanTransformInPlace { get; }

        int GetMaxNumberOutputElements(int inputElementCount);

        bool TryGetTransformedElementCount(ReadOnlySpan<TIn> input, bool isFinalChunk, out int numElementsOutput);

        bool TryTransform(ReadOnlySpan<TIn> input, bool isFinalChunk, Span<TOut> output, out int numElementsRead, out int numElementsWritten);
    }

    public static class TransformExtensions
    {
        public static int GetTransformedElementCount<TIn, TOut>(this ITransform<TIn, TOut> transform, ReadOnlySpan<TIn> input)
        {
            return GetTransformedElementCount<TIn, TOut>(transform, input, isFinalChunk: true);
        }

        public static int GetTransformedElementCount<TIn, TOut>(this ITransform<TIn, TOut> transform, ReadOnlySpan<TIn> input, bool isFinalChunk)
        {
            if (!transform.TryGetTransformedElementCount(input, isFinalChunk, out int numElementsOutput))
            {
                throw new Exception("Error occurred during transform.");
            }
            return numElementsOutput;
        }

        public static TOut[] Transform<TIn, TOut>(this ITransform<TIn, TOut> transform, ReadOnlySpan<TIn> input)
        {
            var output = new TOut[GetTransformedElementCount<TIn, TOut>(transform, input)];
            Transform<TIn, TOut>(transform, input, output, out int numElementsWritten);
            if (numElementsWritten == output.Length)
            {
                return output;
            }
            else
            {
                var trimmedOutput = new TOut[numElementsWritten];
                Array.Copy(output, trimmedOutput, trimmedOutput.Length);
                return trimmedOutput;
            }
        }

        public static void Transform<TIn, TOut>(this ITransform<TIn, TOut> transform, ReadOnlySpan<TIn> input, Span<TOut> output, out int numElementsWritten)
        {
            Transform<TIn, TOut>(transform, input, true /* isFinalChunk */, output, out _, out numElementsWritten);
        }

        public static void Transform<TIn, TOut>(this ITransform<TIn, TOut> transform, ReadOnlySpan<TIn> input, bool isFinalChunk, Span<TOut> output, out int numElementsRead, out int numElementsWritten)
        {
            if (!transform.TryTransform(input, isFinalChunk, output, out numElementsRead, out numElementsWritten))
            {
                throw new Exception("Error occurred during transform.");
            }
        }

        public static int TransformInPlace<TElement>(this ITransform<TElement, TElement> transform, Span<TElement> buffer)
        {
            if (!TryTransformInPlace(transform, buffer, out int numElementsConverted))
            {
                throw new Exception("Error occurred during transform.");
            }
            return numElementsConverted;
        }

        public static bool TryTransformInPlace<TElement>(this ITransform<TElement, TElement> transform, Span<TElement> buffer, out int numElementsConverted)
        {
            return TryTransformInPlace(transform, buffer, true /* isFinalChunk */, out numElementsConverted);
        }

        public static bool TryTransformInPlace<TElement>(this ITransform<TElement, TElement> transform, Span<TElement> buffer, bool isFinalChunk, out int numElementsConverted)
        {
            numElementsConverted = 0;
            return transform.CanTransformInPlace && transform.TryTransform(buffer, isFinalChunk, buffer, out _, out numElementsConverted);
        }
    }



    // ToLower is a text transform: it transforms TextualData -> TextualData
    public class ToLowerTransform : ITransform<char, char>, ITransform<Utf8Char, Utf8Char>
    {
    }

    // Utf8Decode is a BinaryData -> TextualData transform
    public class Utf8DecodeTransform : ITransform<byte, char>, ITransform<byte, Utf8Char>
    {
    }

    // Base64Encode is a BinaryData -> TextualData transform
    public class Base64EncodeTransform : ITransform<byte, char>, ITransform<byte, Utf8Char>
    {
    }

    // Utf8Encode is a TextualData -> BinaryData transform
    public class Utf8EncodeTransform : ITransform<char, byte>, ITransform<Utf8Char, byte>
    {
    }

    // Base64Decode is a TextualData -> BinaryData transform
    public class Base64Decode : ITransform<char, byte>, ITransform<Utf8Char, byte>
    {
    }

    // BitwiseInvert is a BinaryData -> BinaryData transform
    public class BitwiseInvertTransform : ITransform<byte, byte>
    {
    }

    // HtmlEncode is a TextualData -> TextualData transform
    public class HtmlEncodeTransform : ITransform<char, char>, ITransform<Utf8Char, Utf8Char>
    {
    }

    public static class Transforms
    {
        public static readonly BitwiseInvertTransform BitwiseInvert { get; }
        public static readonly HtmlEncodeTransform HtmlEncode { get; }
        public static readonly ToLowerTransform ToLower { get; }
        public static readonly Utf8DecodeTransform Utf8Decode { get; }
        public static readonly Utf8EncodeTransform Utf8Encode { get; }
    }
}
