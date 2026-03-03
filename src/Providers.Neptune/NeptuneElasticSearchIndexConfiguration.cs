namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// Specifies the ElasticSearch index configuration used by Neptune.
    /// </summary>
    public enum NeptuneElasticSearchIndexConfiguration
    {
        /// <summary>
        /// Standard ElasticSearch index configuration with default tokenization.
        /// </summary>
        Standard,

        /// <summary>
        /// Lowercase keyword index configuration that preserves the full property value as a single token.
        /// </summary>
        LowercaseKeyword
    }
}
