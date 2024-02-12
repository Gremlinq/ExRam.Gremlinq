namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal enum WellKnownOperation
    {
        EnumerableIntersectAny,
        EnumerableAny,
        EnumerableContains,

        StringEquals,
        StringContains,
        StringStartsWith,
        StringEndsWith,

        ComparableCompareTo,

        IndexerGet
    }
}
