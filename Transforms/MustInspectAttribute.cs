using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Transforms
{
    /// <summary>
    /// Indicates that the return value of a method should be used by the method's caller
    /// and not simply discarded. Failure to observe the method's return value should be
    /// treated as a compiler warning.
    /// </summary>
    [AttributeUsage(AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = true)]
    public sealed class MustInspectAttribute : Attribute
    {
        private static readonly int HashCode = RuntimeHelpers.GetHashCode(new object());
        
        public override bool Equals(object obj) => (obj is MustInspectAttribute);

        public override int GetHashCode() => HashCode;

        public override bool IsDefaultAttribute() => true;
    }
}
