using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource
    {
        IVGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        IEGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        IVGremlinQuery<IVertex> V(params object[] ids);
        IVGremlinQuery<TVertex> V<TVertex>(params object[] ids);
        IEGremlinQuery<IEdge> E(params object[] ids);
        IEGremlinQuery<TEdge> E<TEdge>(params object[] ids);
        IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);
    }
}
