namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal enum WellKnownOperation
    {
        EnumerableIntersectAny,
        EnumerableAny,
        EnumerableContains,

        ListContains,

        StringEquals,
        StringContains,
        StringStartsWith,
        StringEndsWith,

        ComparableCompareTo,

        IndexerGet
    }
}
