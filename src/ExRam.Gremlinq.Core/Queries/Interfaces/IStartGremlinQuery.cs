namespace ExRam.Gremlinq.Core
{
    public interface IStartGremlinQuery
    {
        IVertexGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        IVertexGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new();

        IEdgeGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        IEdgeGremlinQuery<TEdge> AddE<TEdge>() where TEdge : new();

        IGremlinQueryAdmin AsAdmin();

        IVertexGremlinQuery<object> V(params object[] ids);
        IVertexGremlinQuery<TVertex> V<TVertex>(params object[] ids);

        IValueGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);

        IVertexGremlinQuery<TNewVertex> ReplaceV<TNewVertex>(TNewVertex vertex);
    }
}
