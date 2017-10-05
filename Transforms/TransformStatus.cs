using System;
using System.Collections.Generic;
using System.Text;

namespace Transforms
{
    public enum TransformStatus
    {
        /// <summary>
        /// The input buffer contains invalid data and transformation cannot complete.
        /// The contents of the output buffer and the 'out' method parameters are undefined.
        /// The caller should not attempt to resume the transformation.
        /// </summary>
        Error = -1,

        /// <summary>
        /// All input elements were consumed and transformed into output elements without error.
        /// The caller should inspect the 'numElementsWritten' output parameter.
        /// </summary>
        FullyCompleted = 0,

        /// <summary>
        /// The transform consumed as many elements as possible from the input buffer, but the
        /// transform requires more data in the input buffer or requires a larger output buffer.
        /// The caller should inspect the 'numElementsConsumed' and 'numElementsWritten' output parameters.
        /// This status code is not an error.
        /// </summary>
        Incomplete = 1
    }
}
