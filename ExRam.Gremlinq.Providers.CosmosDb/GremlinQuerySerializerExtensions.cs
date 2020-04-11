using System;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySerializerExtensions
    {
        internal static IGremlinQuerySerializer UseCosmosDbWorkarounds(this IGremlinQuerySerializer serializer)
        {
            return serializer
                .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                    .Override<CosmosDbKey>((key, overridden, recurse) => recurse.Serialize(key.PartitionKey != null ? new[] { key.PartitionKey, key.Id } : (object)key.Id))
                    .Override<HasKeyStep>((step, overridden, recurse) =>
                    {
                        if (step.Argument is P p && (!p.OperatorName.Equals("eq", StringComparison.OrdinalIgnoreCase)))
                            throw new NotSupportedException("CosmosDb does not currently support 'hasKey(P)'.");

                        return overridden(step);
                    })
                    .Override<SkipStep>((step, overridden, recurse) => recurse.Serialize(new RangeStep(step.Count, -1, step.Scope)))
                    .Override<LimitStep>((step, overridden, recurse) =>
                    {
                        // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                        if (step.Count > int.MaxValue)
                            throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Limit' outside the range of a 32-bit-integer.");

                        return overridden(step);
                    })
                    .Override<TailStep>((step, overridden, recurse) =>
                    {
                        // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                        if (step.Count > int.MaxValue)
                            throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Tail' outside the range of a 32-bit-integer.");

                        return overridden(step);
                    })
                    .Override<RangeStep>((step, overridden, recurse) =>
                    {
                        // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                        if (step.Lower > int.MaxValue || step.Upper > int.MaxValue)
                            throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Range' outside the range of a 32-bit-integer.");

                        return overridden(step);
                    })
                    .Override<long>((l, overridden, recurse) =>
                    {
                        // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                        return recurse.Serialize((int)l);
                    }));
        }
    }
}
