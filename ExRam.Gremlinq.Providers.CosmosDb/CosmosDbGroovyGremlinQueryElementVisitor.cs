using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public class CosmosDbGroovyGremlinQueryElementVisitor : GroovyGremlinQueryElementVisitor
    {
        public override void Visit(SkipStep step)
        {
            // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
            if (step.Count > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Skip' outside the range of a 32-bit-integer.");

            base.Visit(step);
        }

        public override void Visit(LimitStep step)
        {
            // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
            if (step.Limit > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Limit' outside the range of a 32-bit-integer.");

            base.Visit(step);
        }

        public override void Visit(TailStep step)
        {
            // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
            if (step.Count > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Tail' outside the range of a 32-bit-integer.");

            base.Visit(step);
        }

        public override void Visit(RangeStep step)
        {
            // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
            if (step.Lower > int.MaxValue || step.Upper > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Range' outside the range of a 32-bit-integer.");

            base.Visit(step);
        }

        public override void Visit(HasStep step)
        {
            if (step.Value is P.Within within && within.Arguments.Length == 0)
                base.Visit(new NotStep(GremlinQuery.Anonymous(GraphModel.Empty).Identity()));
            else
                base.Visit(step);
        }
    }
}
