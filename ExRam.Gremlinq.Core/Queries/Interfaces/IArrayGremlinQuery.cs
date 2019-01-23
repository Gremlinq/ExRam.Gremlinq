namespace ExRam.Gremlinq.Core
{
    public partial interface IArrayGremlinQuery<TArray, TQuery> : IGremlinQuery<TArray>
    {
        TQuery Unfold();
    }
}