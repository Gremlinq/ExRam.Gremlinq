namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Controls how much detail is included in query log output.
    /// </summary>
    [Flags]
    public enum QueryLogVerbosity
    {
        /// <summary>
        /// Log only the query script.
        /// </summary>
        QueryOnly = 0,

        /// <summary>
        /// Include parameter bindings in the log output.
        /// </summary>
        IncludeBindings = 1
    }
}
