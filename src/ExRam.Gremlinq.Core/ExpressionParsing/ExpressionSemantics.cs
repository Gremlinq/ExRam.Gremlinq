namespace ExRam.Gremlinq.Core
{
    internal enum ExpressionSemantics
    {
        False,
        True,
        LowerThan,
        LowerThanOrEqual,
        Equals,
        GreaterThanOrEqual,
        GreaterThan,
        NotEquals,
        Intersects,
        Contains,
        HasInfix,
        StartsWith,
        EndsWith,
        
        IsContainedIn,
        IsInfixOf,
        IsPrefixOf,
        IsSuffixOf
    }
}
