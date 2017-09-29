namespace System
{
    public ref struct Utf8StringView
    {
        public Utf8StringView(Utf8String value);
        public Utf8StringView(Utf8String value, int offset, int count);
        public Utf8StringView(Utf8StringView value);
        public Utf8StringView(Utf8StringView value, int offset, int count);
        public Utf8StringView(ReadOnlySpan<Utf8Char> value);

        // Equals, IndexOf, SubStr, iterators, and all forms of goodness

        public ReadOnlySpan<Utf8Char> AsSpan();
    }
}
