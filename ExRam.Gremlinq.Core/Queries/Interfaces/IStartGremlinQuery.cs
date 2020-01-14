namespace ExRam.Gremlinq.Core
{
    public interface IStartGremlinQuery
    {
        IVertexGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        IEdgeGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        IVertexGremlinQuery<object> V(params object[] ids);
        IEdgeGremlinQuery<object> E(params object[] ids);
        IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);

        IVertexGremlinQuery<TNewVertex> ReplaceV<TNewVertex>(TNewVertex vertex);
        IEdgeGremlinQuery<TNewEdge> ReplaceE<TNewEdge>(TNewEdge edge);
    }
}
