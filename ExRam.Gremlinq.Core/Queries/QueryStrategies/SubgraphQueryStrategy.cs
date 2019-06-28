using System;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class SubgraphQueryStrategy : IGremlinQueryStrategy
    {
        private readonly Func<IEdgeGremlinQuery<IEdge>, IGremlinQuery> _edgeCriterion;
        private readonly Func<IVertexGremlinQuery<IVertex>, IGremlinQuery> _vertexCriterion;

        public SubgraphQueryStrategy(Func<IVertexGremlinQuery<IVertex>, IGremlinQuery> vertexCriterion, Func<IEdgeGremlinQuery<IEdge>, IGremlinQuery> edgeCriterion)
        {
            _edgeCriterion = edgeCriterion;
            _vertexCriterion = vertexCriterion;
        }

        public IGremlinQuery Apply(IGremlinQuery query)
        {
            var admin = query.AsAdmin();
            var anonymous = GremlinQuery.Anonymous(admin);

            var vertexCriterionTraversal = _vertexCriterion(anonymous.AsAdmin().ChangeQueryType<IVertexGremlinQuery<IVertex>>());
            var edgeCriterionTraversal = _edgeCriterion(anonymous.AsAdmin().ChangeQueryType<IEdgeGremlinQuery<IEdge>>());

            if (vertexCriterionTraversal.AsAdmin().Steps.Count > 0 || edgeCriterionTraversal.AsAdmin().Steps.Count > 0)
            {
                var strategy = GremlinQuery.Create<Unit>("SubgraphStrategy", admin)
                    .AddStep(BuildStep.Instance);

                if (vertexCriterionTraversal.AsAdmin().Steps.Count > 0)
                {
                    strategy = strategy.AddStep(new VerticesStep(vertexCriterionTraversal));
                }

                if (edgeCriterionTraversal.AsAdmin().Steps.Count > 0)
                {
                    strategy = strategy.AddStep(new EdgesStep(edgeCriterionTraversal));
                }

                return query
                    .AddStep(new WithStrategiesStep(strategy.AddStep(CreateStep.Instance)));
            }

            return query;
        }
    }
}
