namespace ExRam.Gremlinq.Core
{
    [Obsolete("Starting from version 14, Gremlinq will always behave as if StringComparisonTranslationStrictness.Strict was configured. Queries using a string comparison which is not supported on a specific database provider (e.g. case insensitive queries on Azure CosmosDb) must be modified accordingly.")]
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
