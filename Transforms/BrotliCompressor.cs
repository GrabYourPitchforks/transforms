using System;
using System.Collections.Generic;
using System.Text;

namespace Transforms
{
    public class BrotliCompressor : IStatefulTransformFactory<byte, byte>
    {
        public IStatefulTransform<byte, byte> CreateTransform()
        {
            throw new NotImplementedException();
        }

        public bool TryGetMaxOutputElementCount(int numInputElements, out int maxOutputElementCount)
        {
            throw new NotImplementedException();
        }
    }
}
