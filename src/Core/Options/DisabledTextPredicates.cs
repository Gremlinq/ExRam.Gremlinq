namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Specifies which text predicates are not supported by the target graph database.
    /// Disabled predicates will cause an exception when used in queries.
    /// </summary>
    [Flags]
    public enum DisabledTextPredicates
    {
        /// <summary>
        /// All text predicates are enabled.
        /// </summary>
        None = 0,
        Containing = 1,
        EndingWith = 2,
        NotContaining = 4,
        NotEndingWith = 8,
        NotStartingWith = 16,
        StartingWith = 32,
        Regex = 64,
        NotRegex = 128
    }
}
