using System;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySerializerExtensions
    {
        internal static IGremlinQuerySerializer UseCosmosDbWorkarounds(this IGremlinQuerySerializer serializer)
        {
            return serializer
                .OverrideFragmentSerializer<CosmosDbKey>((key, overridden, recurse) => recurse(key.PartitionKey != null ? new[] { key.PartitionKey, key.Id } : (object)key.Id))
                .OverrideFragmentSerializer<SkipStep>((step, overridden, recurse) => recurse(new RangeStep(step.Count, -1)))
                .OverrideFragmentSerializer<LimitStep>((step, overridden, recurse) =>
                {
                    // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                    if (step.Count > int.MaxValue)
                        throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Limit' outside the range of a 32-bit-integer.");

                    return overridden(step);
                })
                .OverrideFragmentSerializer<TailStep>((step, overridden, recurse) =>
                {
                    // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                    if (step.Count > int.MaxValue)
                        throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Tail' outside the range of a 32-bit-integer.");

                    return overridden(step);
                })
                .OverrideFragmentSerializer<RangeStep>((step, overridden, recurse) =>
                {
                    // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                    if (step.Lower > int.MaxValue || step.Upper > int.MaxValue)
                        throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Range' outside the range of a 32-bit-integer.");

                    return overridden(step);
                })
                .OverrideFragmentSerializer<long>((l, overridden, recurse) =>
                {
                    // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                    return recurse((int)l);
                });
        }
    }
}
