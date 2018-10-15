using System;
using LanguageExt;

namespace ExRam.Gremlinq
{
    // ReSharper disable once InconsistentNaming
    public static class g
    {
        // ReSharper disable once UnusedMember.Global
        internal static readonly IGremlinQuery<Unit> G = GremlinQuery.Create("g");

        public static IGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex)
        {
            return G.AddV(vertex);
        }

        public static IGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new()
        {
            return G.AddV<TVertex>();
        }

        public static IVGremlinQuery<Vertex> V(params object[] ids)
        {
            return G.V(ids);
        }

        public static IVGremlinQuery<TVertex> V<TVertex>(params object[] ids)
        {
            return G.V<TVertex>(ids);
        }

        public static IGremlinQuery<Edge> E(params object[] ids)
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
    }
}
