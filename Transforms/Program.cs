using System;

public static class Program
{
    public static void Main()
    {
        // Pipelining sample. Given an input string "ABCDEF&abcdef":
        // 1) Convert it to lowercase, then
        // 2) HTML-encode it, then
        // 3) Convert it to UTF8, then
        // 4) Bitwise invert the result

        //
        // FROM SYSTEM.STRING
        //

        Span<char> tempBuffer = stackalloc char[256];
        int numCharsWritten = Transforms.ToLower.Transform("ABCDEF&abcdef", tempBuffer);
        numCharsWritten = Transforms.HtmlEncode.Transform(tempBuffer.Slice(0, numCharsWritten), tempBuffer);

        Span<byte> byteBuffer = stackalloc byte[256];
        int numBytesWritten = Transforms.Utf8Encode.Transform(tempBuffer.Slice(0, numCharsWritten), byteBuffer);
        numBytesWritten = Transforms.BitwiseInvert.Transform(byteBuffer.Slice(0, numBytesWritten), byteBuffer);

        // Send byteBuffer.Slice(0, numBytesWritten) across the wire.

        //
        // FROM SYSTEM.UTF8STRING
        //
        
        Span<Utf8Char> tempBuffer = stackalloc Utf8Char[256];
        int numCharsWritten = Transforms.ToLower.Transform("ABCDEF&abcdef", tempBuffer);
        numCharsWritten = Transforms.HtmlEncode.Transform(tempBuffer.Slice(0, numCharsWritten), tempBuffer);

        Span<byte> byteBuffer = stackalloc byte[256];
        int numBytesWritten = Transforms.Utf8Encode.Transform(tempBuffer.Slice(0, numCharsWritten), byteBuffer);
        numBytesWritten = Transforms.BitwiseInvert.Transform(byteBuffer.Slice(0, numBytesWritten), byteBuffer);

        // Or, if you reallllly wanted to optimize (and we should strongly discourage this):

        Span<Utf8Char> tempBuffer = stackalloc Utf8Char[256];
        int numCharsWritten = Transforms.ToLower.Transform("ABCDEF&abcdef", tempBuffer);
        numCharsWritten = Transforms.HtmlEncode.Transform(tempBuffer.Slice(0, numCharsWritten), tempBuffer);

        Span<byte> byteBuffer = tempBuffer.AsBytes(); // already known to be in utf8, avoid the memcpy
        int numBytesWritten = numCharsWritten;
        numBytesWritten = Transforms.BitwiseInvert.Transform(byteBuffer.Slice(0, numBytesWritten), byteBuffer);

        // Send byteBuffer.Slice(0, numBytesWritten) across the wire.

        //
        // OR, USING PIPELINES
        //

        var pipelineTranform1 = new PipelineTransform<char, char>(Transforms.ToLower)
            .Append(Transforms.HtmlEncode)
            .Append(Transforms.Utf8Encode)
            .Append(Transforms.BitwiseInvert);
        var /* byte[] */ output1 = pipelineTranform1.Transform("ABCDEF&abcdef");

        var pipelineTranform2 = new PipelineTransform<Utf8Char, Utf8Char>(Transforms.ToLower)
            .Append(Transforms.HtmlEncode)
            .Append(Transforms.Utf8Encode)
            .Append(Transforms.BitwiseInvert);
        var /* byte[] */ output2 = pipelineTranform2.Transform(u"ABCDEF&abcdef");
    }
}
