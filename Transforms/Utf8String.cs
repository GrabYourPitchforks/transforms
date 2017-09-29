using System.Collections.Generic;

namespace System
{
    public sealed class Utf8String : IEnumerable<Utf8String>, IComparable<Utf8String>, IEquatable<String>
    {
        public static readonly Utf8String Empty;
        public Utf8String(Utf8Char* value);
        public Utf8String(Utf8Char[] value);
        public Utf8String(ReadOnlySpan<Utf8Char> value);
        public Utf8String(Utf8Char c, int count);
        public Utf8String(Utf8Char* value, int startIndex, int length);
        public Utf8String(Utf8Char[] value, int startIndex, int length);
        public Utf8Char this[int index] { get; }
        public int Length { get; }

        // What would the comparison functions look like?
        public static int Compare(...);
        public static int CompareOrdinal(...);
        public static Utf8String Concat(params Utf8String[] values);
        public static Utf8String Concat(...);
        public static bool Equals(Utf8String a, Utf8String b, StringComparison comparisonType);
        public static bool Equals(Utf8String a, Utf8String b);
        public static String Format(IFormatProvider provider, ...);
        public static bool IsNullOrEmpty(Utf8String value);

        public static bool IsNullOrWhiteSpace(Utf8String value);
        public static Utf8String Join(...);
        public int CompareTo(Utf8String strB);
        public bool Contains(Utf8String value);
        public bool Contains(Utf8String value, ...);
        public void CopyTo(int sourceIndex, Utf8Char[] destination, int destinationIndex, int count);
        public bool EndsWith(Utf8Char value);
        public bool EndsWith(Utf8String value, ...);
        public bool Equals(Utf8String value, ...);
        public Utf8CharEnumerator GetEnumerator();
        public Utf8CodePointEnumerable AsCodePointEnumerable();
        public override int GetHashCode();
        public int IndexOf(Utf8String value, ...);

        // and so on...
    }
}
