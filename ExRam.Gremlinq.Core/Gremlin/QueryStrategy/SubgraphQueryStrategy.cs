using System;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class SubgraphQueryStrategy : IGremlinQueryStrategy
    {
        private readonly Func<IEGremlinQuery<IEdge>, IGremlinQuery> _edgeCriterion;
        private readonly Func<IVGremlinQuery<IVertex>, IGremlinQuery> _vertexCriterion;

        public SubgraphQueryStrategy(Func<IVGremlinQuery<IVertex>, IGremlinQuery> vertexCriterion, Func<IEGremlinQuery<IEdge>, IGremlinQuery> edgeCriterion)
        {
            _edgeCriterion = edgeCriterion;
            _vertexCriterion = vertexCriterion;
        }

        public IGremlinQuery Apply(IGremlinQuery query)
        {
            var admin = query.AsAdmin();
            var anonymous = GremlinQuery.Anonymous(admin.Model);

            var vertexCriterionTraversal = _vertexCriterion(anonymous.AsAdmin().ChangeQueryType<IVGremlinQuery<IVertex>>());
            var edgeCriterionTraversal = _edgeCriterion(anonymous.AsAdmin().ChangeQueryType<IEGremlinQuery<IEdge>>());

            if (vertexCriterionTraversal.AsAdmin().Steps.Count > 0 || edgeCriterionTraversal.AsAdmin().Steps.Count > 0)
            {
                var strategy = GremlinQuery.Create<Unit>(admin.Model, GremlinQueryExecutor.Invalid, "SubgraphStrategy")
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
