using System;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class SubgraphQueryStrategy : IGremlinQueryStrategy
    {
        private readonly Func<IEdgeGremlinQuery<object>, IGremlinQuery> _edgeCriterion;
        private readonly Func<IVertexGremlinQuery<object>, IGremlinQuery> _vertexCriterion;

        public SubgraphQueryStrategy(Func<IVertexGremlinQuery<object>, IGremlinQuery> vertexCriterion, Func<IEdgeGremlinQuery<object>, IGremlinQuery> edgeCriterion)
        {
            _edgeCriterion = edgeCriterion;
            _vertexCriterion = vertexCriterion;
        }

        public IGremlinQuery Apply(IGremlinQuery query)
        {
            var environment = query.AsAdmin().Environment;
            var anonymous = GremlinQuery.Anonymous(environment);

            var vertexCriterionTraversal = _vertexCriterion(anonymous.AsAdmin().ChangeQueryType<IVertexGremlinQuery<object>>());
            var edgeCriterionTraversal = _edgeCriterion(anonymous.AsAdmin().ChangeQueryType<IEdgeGremlinQuery<object>>());

            if (vertexCriterionTraversal.AsAdmin().Steps.Count > 0 || edgeCriterionTraversal.AsAdmin().Steps.Count > 0)
            {
                var strategy = GremlinQuery.Create<object>(environment)
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
