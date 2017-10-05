using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Transforms
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Utf8Char
    {
        private byte Value;
    }
}
