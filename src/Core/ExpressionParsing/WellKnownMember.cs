namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal enum WellKnownOperation
    {
        Equals,

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

    internal enum WellKnownMember
    {
        PropertyValue,
        PropertyKey,
        StepLabelValue,
        VertexPropertyLabel,

        ArrayLength,
    }
}
