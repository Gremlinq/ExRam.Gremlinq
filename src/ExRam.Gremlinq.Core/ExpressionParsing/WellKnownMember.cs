namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal enum WellKnownMember
    {
        Equals,

        PropertyValue,
        PropertyKey,
        StepLabelValue,
        VertexPropertyLabel,
        
        EnumerableIntersectAny,
        EnumerableAny,
        EnumerableContains,

        ListContains,

        StringEquals,
        StringContains,
        StringStartsWith,
        StringEndsWith,

        ComparableCompareTo,

        ArrayLength
    }
}
