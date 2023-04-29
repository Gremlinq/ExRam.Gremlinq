namespace ExRam.Gremlinq.Core
{
    public enum StringComparisonTranslationStrictness
    {
        // When an Expression contains a StringComparison value that is not supported by the database provider,
        // (such as StringComparison.OrdinalIgnoreCase when the database provider does not support case-insensitive
        // lookups), an exception will be thrown.
        Strict = 0,

        // When an Expression contains a StringComparison value that is not supported by the database provider,
        // (such as StringComparison.OrdinalIgnoreCase when the database provider does not support case-insensitive
        // lookups), it will be silently ignored.
        Lenient = 1
    }
}
