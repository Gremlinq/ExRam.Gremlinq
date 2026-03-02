namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Controls whether Gremlinq includes all label filters or omits them when they match all known types.
    /// </summary>
    public enum FilterLabelsVerbosity
    {
        /// <summary>
        /// Always includes all appropriate labels in filter steps (e.g. <c>.out('l1', ...)</c>),
        /// even when the label set matches all known types. This is the default and most conservative
        /// option since the database may contain elements with labels unknown to Gremlinq.
        /// </summary>
        Maximum = 0,

        /// <summary>
        /// Omits label filters when they would include all types known to Gremlinq,
        /// enabling shorter syntax (e.g. <c>.out()</c> instead of <c>.out('l1', ...)</c>).
        /// Assumes no elements with unknown labels exist in the database.
        /// </summary>
        Minimum = 1
    }
}
