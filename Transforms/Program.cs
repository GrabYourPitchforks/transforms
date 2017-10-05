using System;
using System.Diagnostics;
using System.IO;
using Transforms;

public static class Program
{
    public static void Main()
    {
        byte[] b = { 1, 2, 3, 4, 5, 0xfe };
        char[] cs = new char[40];

        new HexEncoder(true).Transform(b, cs, true, out _, out _);
    }

    private static void TransformStreams(IStatelessTransform<byte, byte> transform, Stream inputStream, Stream outputStream)
    {
        Span<byte> readBuffer = new byte[4096]; // or stackalloc
        Span<byte> writeBuffer = new byte[4096]; // or stackalloc
        Span<byte> availableInputData = Span<byte>.Empty;

        while (true)
        {
            // If there's any available input data, move it to the beginning of the read buffer
            // so that data coming from the stream can be written to the end of the buffer.

            availableInputData.CopyTo(readBuffer); // this is a memmove instead of a memcpy, right? supports overlapping spans?
            availableInputData = readBuffer.Slice(0, availableInputData.Length);
            var emptyRegionAtEndOfReadBuffer = readBuffer.Slice(availableInputData.Length);

            // At this point, 'availableInputData' and 'emptyRegionAtEndOfReadBuffer' are adjacent
            // spans which when combined overlap perfectly with 'readBuffer'.

            if (emptyRegionAtEndOfReadBuffer.IsEmpty)
            {
                // Something went horribly wrong in a previous iteration of the loop.
                // The underlying transform failed to make forward progress, and there's no more
                // room for us to read any more data from the input stream. If we don't bail
                // now we'll end up in an infinite loop.
                Debug.Fail("This should never happen. The transform must always make forward progress.");
                throw new Exception("Logic error in transform routine.");
            }

            int numBytesReadThisIteration = inputStream.Read(emptyRegionAtEndOfReadBuffer);
            if (numBytesReadThisIteration == 0)
            {
                break; // Stream EOF reached; jump to EOF handling outside of loop
            }

            // If we reached this point, we haven't yet reached EOF on the input stream.
            // Transform what we can.

            availableInputData = readBuffer.Slice(0, availableInputData.Length + numBytesReadThisIteration);

            // n.b. I don't care about any status code other than "error" or "not error"
            if (transform.Transform(availableInputData, writeBuffer, false /* isFinalChunk */, out int numElementsConsumed, out int numElementsWritten) == TransformStatus.Error)
            {
                throw new Exception("Error occurred during transform.");
            }

            // Copy transformed data to output.

            outputStream.Write(writeBuffer.Slice(0, numElementsWritten));
        }

        // At this point, we've reached EOF in the input stream.
        // If there's any data for us left to transform, we must force it through now.

        while (!availableInputData.IsEmpty)
        {
            // n.b. I don't care about any status code other than "error" or "not error"
            if (transform.Transform(availableInputData, writeBuffer, true /* isFinalChunk */, out int numElementsConsumed, out int numElementsWritten) == TransformStatus.Error)
            {
                throw new Exception("Error occurred during transform.");
            }
            else if (numElementsConsumed == 0)
            {
                Debug.Fail("This should never happen. The transform must always make forward progress.");
                throw new Exception("Logic error in transform routine.");
            }
            else
            {
                // write out what we've transformed so far and loop
                outputStream.Write(writeBuffer.Slice(0, numElementsWritten));
                availableInputData = availableInputData.Slice(numElementsConsumed);
            }
        }

        // And now we're finished!
    }
}
