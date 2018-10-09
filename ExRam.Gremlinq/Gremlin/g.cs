using System;
using LanguageExt;

namespace ExRam.Gremlinq
{
    // ReSharper disable once InconsistentNaming
    public static class g
    {
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once RedundantArgumentDefaultValue
        internal static readonly IGremlinQuery<Unit> _g = GremlinQuery.Create("g");

        public static IGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex)
        {
            return _g.AddV(vertex);
        }

        public static IGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new()
        {
            return _g.AddV<TVertex>();
        }

        public static IGremlinQuery<Vertex> V(params object[] ids)
        {
            return _g.V(ids);
        }

        public static IGremlinQuery<TVertex> V<TVertex>(params object[] ids)
        {
            return _g.V<TVertex>(ids);
        }

        public static IGremlinQuery<Edge> E(params object[] ids)
        {
            return _g.E(ids);
        }

        public static IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements)
        {
            return _g
                .Cast<TElement>()
                .Inject(elements);
        }

        public static IGremlinQuery<Unit> WithSubgraphStrategy(Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion)
        {
            return _g
                .WithSubgraphStrategy(vertexCriterion, edgeCriterion);
        }

        public static IGremlinQuery<Unit> SetModel(IGraphModel model)
        {
            return _g
                .SetModel(model);
        }
    }
}