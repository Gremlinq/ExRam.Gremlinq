using System;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public sealed class SubgraphQueryStrategy : IGremlinQueryStrategy
    {
        private readonly Func<IGremlinQuery<Unit>, IGremlinQuery> _edgeCriterion;
        private readonly Func<IGremlinQuery<Unit>, IGremlinQuery> _vertexCriterion;

        public SubgraphQueryStrategy(Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion)
        {
            _edgeCriterion = edgeCriterion;
            _vertexCriterion = vertexCriterion;
        }

        public IGremlinQuery Apply(IGremlinQuery query)
        {
            var admin = query.AsAdmin();
            var anonymous = GremlinQuery.Anonymous(admin.Model);

            var vertexCriterionTraversal = _vertexCriterion(anonymous);
            var edgeCriterionTraversal = _edgeCriterion(anonymous);

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
