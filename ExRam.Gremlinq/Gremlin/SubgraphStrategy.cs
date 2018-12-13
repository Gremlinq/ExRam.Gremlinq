using System;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class SubgraphStrategy : IGremlinQueryStrategy
    {
        private readonly Func<IGremlinQuery<Unit>, IGremlinQuery> _edgeCriterion;
        private readonly Func<IGremlinQuery<Unit>, IGremlinQuery> _vertexCriterion;

        public SubgraphStrategy(Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion)
        {
            _edgeCriterion = edgeCriterion;
            _vertexCriterion = vertexCriterion;
        }

        public IGremlinQuery<TElement> Apply<TElement>(IGremlinQuery<TElement> query)
        {
            var anonymous = GremlinQuery.Anonymous(query.Model);

            var vertexCriterionTraversal = _vertexCriterion(anonymous);
            var edgeCriterionTraversal = _edgeCriterion(anonymous);

            if (vertexCriterionTraversal.Steps.Count > 1 || edgeCriterionTraversal.Steps.Count > 1)
            {
                var strategy = GremlinQuery.Create<Unit>(query.Model, GremlinQueryProvider.Invalid, "SubgraphStrategy")
                    .AddStep(BuildStep.Instance);

                if (vertexCriterionTraversal.Steps.Count > 0)
                {
                    strategy = strategy.AddStep(new VerticesStep(vertexCriterionTraversal));
                }

                if (edgeCriterionTraversal.Steps.Count > 0)
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
