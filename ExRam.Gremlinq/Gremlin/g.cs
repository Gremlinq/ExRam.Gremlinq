using System;
using LanguageExt;

namespace ExRam.Gremlinq
{
    // ReSharper disable once InconsistentNaming
    public static class g
    {
        // ReSharper disable once UnusedMember.Global
        internal static readonly IGremlinQuery<Unit> G = GremlinQuery.Create("g");

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
                .Cast<TElement>()
                .Inject(elements);
        }

        public static IGremlinQuery<Unit> WithSubgraphStrategy(Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion)
        {
            return G
                .WithSubgraphStrategy(vertexCriterion, edgeCriterion);
        }

        public static IGremlinQuery<Unit> SetQueryProvider(IGremlinQueryProvider gremlinQueryProvider)
        {
            return G
                .SetTypedGremlinQueryProvider(gremlinQueryProvider);
        }
    }
}
