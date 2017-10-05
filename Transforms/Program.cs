using System;
using Transforms;

public static class Program
{
    public static void Main()
    {
        byte[] b = { 1, 2, 3, 4, 5, 0xfe };
        char[] cs = new char[40];

        new HexEncoder(true).Transform(b, cs, true, out _, out _);
    }
}
