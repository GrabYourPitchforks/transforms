using System;
using System.Collections.Generic;
using System.Text;

namespace Transforms
{
    public interface IStatefulTransformFactory<TIn, TOut>
    {
        bool TryGetMaxOutputElementCount(int numInputElements, out int maxOutputElementCount);

        IStatefulTransform<TIn, TOut> CreateTransform();
    }
}
