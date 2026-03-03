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
        /// <summary>Disables the <c>TextP.containing()</c> predicate.</summary>
        Containing = 1,
        /// <summary>Disables the <c>TextP.endingWith()</c> predicate.</summary>
        EndingWith = 2,
        /// <summary>Disables the <c>TextP.notContaining()</c> predicate.</summary>
        NotContaining = 4,
        /// <summary>Disables the <c>TextP.notEndingWith()</c> predicate.</summary>
        NotEndingWith = 8,
        /// <summary>Disables the <c>TextP.notStartingWith()</c> predicate.</summary>
        NotStartingWith = 16,
        /// <summary>Disables the <c>TextP.startingWith()</c> predicate.</summary>
        StartingWith = 32,
        /// <summary>Disables the <c>TextP.regex()</c> predicate.</summary>
        Regex = 64,
        /// <summary>Disables the <c>TextP.notRegex()</c> predicate.</summary>
        NotRegex = 128
    }
}
