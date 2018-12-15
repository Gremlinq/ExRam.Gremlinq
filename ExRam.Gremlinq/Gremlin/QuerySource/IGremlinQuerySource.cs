using ExRam.Gremlinq.GraphElements;

namespace ExRam.Gremlinq
{
    public interface IGremlinQuerySource
    {
        IVGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        IVGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new();
        IEGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        IEGremlinQuery<TEdge> AddE<TEdge>() where TEdge : new();
        IVGremlinQuery<Vertex> V(params object[] ids);
        IVGremlinQuery<TVertex> V<TVertex>(params object[] ids);
        IEGremlinQuery<Edge> E(params object[] ids);
        IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);

        IGremlinQuerySource WithStrategies(params IGremlinQueryStrategy[] strategies);
        IGremlinQuerySource WithModel(IGraphModel model);
        IGremlinQuerySource WithExecutor(IGremlinQueryExecutor executor);
    }
}
