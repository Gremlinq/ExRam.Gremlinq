using System;
using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class SkipStep : Step
    {
        public SkipStep(long count)
        {
            // This is the easier workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public long Count { get; }
    }
}
