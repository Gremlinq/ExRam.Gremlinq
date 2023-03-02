using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    internal static class CosmosDbTestExtensions
    {
        public static IGremlinQueryEnvironment AddFakePartitionKey(this IGremlinQueryEnvironment env)
        {
            return env
                .ConfigureSerializer(serializer => serializer
                    .Add<AddVStep>((step, env, defer, recurse) => new[]
                    {
                        defer.TransformTo<object>().From(step, env),
                        recurse.Serialize(new PropertyStep.ByKeyStep("PartitionKey", "PartitionKey"), env)
                    }));
        }
    }
}
