using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource
    {
        IVertexGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        IEdgeGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        IVertexGremlinQuery<IVertex> V(params object[] ids);
        IVertexGremlinQuery<TVertex> V<TVertex>(params object[] ids);
        IEdgeGremlinQuery<IEdge> E(params object[] ids);
        IEdgeGremlinQuery<TEdge> E<TEdge>(params object[] ids);
        IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);
    }
}
