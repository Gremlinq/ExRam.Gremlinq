namespace ExRam.Gremlinq.Core
{
    public enum FilterLabelsVerbosity
    {
        // Includes all appropriate labels in a map-step, e.g. ".out('l1', ...)", even if the set
        // of labels equals all possible labels, enabling the much shorter syntax ".out()".
        // This is the default and pessimistic // option since in an actual database, there might
        // exist elements with labels unknown to Gremlinq that would otherwise be incorrectly included
        // in the query results.
        Maximum = 0,

        // Assume there are no elements with labels unknown to Gremlinq in the queried database,
        // enabling Gremlinq to use e.g. "out()" when the more verbose syntax ".out('l1'...)" would
        // include all labels known to Gremlinq.
        Minimum = 1
    }
}