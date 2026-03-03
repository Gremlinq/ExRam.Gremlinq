namespace ExRam.Gremlinq.Core
{
    /// <summary>Controls strictness of string comparison translation.</summary>
    [Obsolete("Starting from version 14, Gremlinq will always behave as if StringComparisonTranslationStrictness.Strict was configured. Queries using a string comparison which is not supported on a specific database provider (e.g. case insensitive queries on Azure CosmosDb) must be modified accordingly.")]
    public enum StringComparisonTranslationStrictness
    {
        /// <summary>Throws an exception when an unsupported <see cref="StringComparison"/> mode is used.</summary>
        Strict = 0,

        /// <summary>Silently ignores unsupported <see cref="StringComparison"/> modes.</summary>
        Lenient = 1
    }
}
