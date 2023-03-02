using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    internal static class CosmosDbTestExtensions
    {
        public static IGremlinQueryEnvironment AddFakePartitionKey(this IGremlinQueryEnvironment env)
        {
            return env
                .ConfigureSerializer(serializer => serializer
                    .Add<AddVStep>((step, env, recurse) => new[]
                    {
                        step,
                        recurse.Serialize(new PropertyStep.ByKeyStep("PartitionKey", "PartitionKey"), env)
                    }));
        }
    }
}
