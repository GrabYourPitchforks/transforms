using System.Runtime.InteropServices;

namespace System
{
    // An 8-bit type similar to but distinct from System.Byte.
    // Utf8Char is not integral (no arithmetic operations) or binary (no bitwise operations)
    // but is comparable (allow ==, <, etc.).
    [StructLayout(LayoutKind.Auto, Size = 1)]
    public struct Utf8Char : IComparable<Utf8Char>, IEquatable<Utf8Char>
    {
        private readonly byte _value;

        public Utf8Char(Utf8Char other)
        {
            _value = other._value;
        }

        public static bool operator ==(Utf8Char a, Utf8Char b) => a._value == b._value;

        public static bool operator !=(Utf8Char a, Utf8Char b) => a._value != b._value;

        public static bool operator <(Utf8Char a, Utf8Char b) => a._value < b._value;

        public static bool operator <=(Utf8Char a, Utf8Char b) => a._value <= b._value;

        public static bool operator >(Utf8Char a, Utf8Char b) => a._value > b._value;

        public static bool operator >=(Utf8Char a, Utf8Char b) => a._value >= b._value;

        public static implicit operator byte(Utf8Char value) => value._value;

        public static implicit operator Utf8Char(byte value) => new Utf8Char(value);

        // other implicit conversions go here
        // ideally this would be an intrinsic type and thus conversions can be properly checked or unchecked

        public int CompareTo(Utf8Char other)
        {
            return this._value.CompareTo(other._value);
        }

        public override bool Equals(object other)
        {
            return (other is Utf8Char) && (this == (Utf8Char)other);
        }

        public bool Equals(Utf8Char other)
        {
            return (this == other);
        }

        public override int GetHashCode()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
