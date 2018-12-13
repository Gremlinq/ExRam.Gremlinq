using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    // ReSharper disable once InconsistentNaming
    public static class g
    {
        // ReSharper disable once UnusedMember.Global
        internal static readonly IGremlinQuerySource G = new GremlinQuerySource("g", GraphModel.Empty, null, ImmutableList<IGremlinQueryStrategy>.Empty);

        public static IVGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex)
        {
            return G.AddV(vertex);
        }

        public static IVGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new()
        {
            return G.AddV<TVertex>();
        }

        public static IEGremlinQuery<TEdge> AddE<TEdge>(TEdge edge)
        {
            return G.AddE(edge);
        }

        public static IEGremlinQuery<TEdge> AddE<TEdge>() where TEdge : new()
        {
            return G.AddE<TEdge>();
        }

        public static IVGremlinQuery<Vertex> V(params object[] ids)
        {
            return G.V(ids);
        }

        public static IVGremlinQuery<TVertex> V<TVertex>(params object[] ids)
        {
            return G.V<TVertex>(ids);
        }

        public static IEGremlinQuery<Edge> E(params object[] ids)
        {
            return G.E(ids);
        }

        public static IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements)
        {
            return G
                .Inject(elements);
        }

        public static IGremlinQuerySource WithModel(IGraphModel model)
        {
            return G
                .WithModel(model);
        }

        public static IGremlinQuerySource WithStrategies(params IGremlinQueryStrategy[] strategies)
        {
            return G
                .WithStrategies(strategies);
        }
        
        public static IGremlinQuerySource WithQueryProvider(IGremlinQueryProvider gremlinQueryProvider)
        {
            return G
                .WithQueryProvider(gremlinQueryProvider);
        }
    }
}
