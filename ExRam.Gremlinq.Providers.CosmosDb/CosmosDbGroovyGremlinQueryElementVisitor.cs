using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public class CosmosDbGroovyGremlinQueryElementVisitor : GroovyGremlinQueryElementVisitor
    {
        private static readonly Step NoneWorkaround = new NotStep(GremlinQuery.Anonymous(GraphModel.Empty).Identity());

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
            if (step.Count > int.MaxValue)
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

        protected override void Method(string methodName, object parameter)
        {
            base.Method(
                methodName,
                parameter is long l ? (int)l : parameter);
        }

        protected override void Method(string methodName, object parameter1, object parameter2)
        {
            base.Method(
                methodName,
                parameter1 is long l1 ? (int)l1 : parameter1,
                parameter2 is long l2 ? (int)l2 : parameter2);
        }

        protected override void Method(string methodName, object parameter1, object parameter2, object parameter3)
        {
            base.Method(
                methodName,
                parameter1 is long l1 ? (int)l1 : parameter1,
                parameter2 is long l2 ? (int)l2 : parameter2,
                parameter2 is long l3 ? (int)l3 : parameter3);
        }

        public override void Visit(NoneStep step)
        {
            Visit(NoneWorkaround);
        }
    }
}
