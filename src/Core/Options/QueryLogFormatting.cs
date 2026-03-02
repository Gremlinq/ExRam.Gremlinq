namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Controls the formatting of query log output.
    /// </summary>
    [Flags]
    public enum QueryLogFormatting
    {
        /// <summary>
        /// No special formatting.
        /// </summary>
        None = 0,

        /// <summary>
        /// Use indented formatting for readability.
        /// </summary>
        Indented = 1
    }
}
