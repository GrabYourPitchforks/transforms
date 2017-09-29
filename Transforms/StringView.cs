namespace System
{
    public ref struct StringView
    {
        public StringView(String value);
        public StringView(String value, int offset, int count);
        public StringView(StringView value);
        public StringView(StringView value, int offset, int count);
        public StringView(ReadOnlySpan<char> value);

        // Equals, IndexOf, SubStr, iterators, and all forms of goodness

        public ReadOnlySpan<char> AsSpan();
    }
}
