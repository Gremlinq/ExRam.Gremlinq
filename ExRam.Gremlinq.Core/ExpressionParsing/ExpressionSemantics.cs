namespace ExRam.Gremlinq.Core
{
    internal enum ExpressionSemantics
    {
        Equals,
        LowerThan,
        LowerThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        Intersects,
        Contains,
        HasInfix,
        StartsWith,
        EndsWith,
        NotEquals,
        IsContainedIn,
        IsInfixOf,
        IsPrefixOf,
        IsSuffixOf,
    }
}