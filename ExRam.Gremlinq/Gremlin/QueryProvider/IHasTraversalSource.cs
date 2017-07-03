namespace ExRam.Gremlinq
{
    public interface IHasTraversalSource
    {
        IGremlinQuery TraversalSource { get; }
    }
}