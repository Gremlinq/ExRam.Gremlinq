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
            var threadLocal = new ThreadLocal<AddVStep?>();

            return env
                .ConfigureSerializer(serializer => serializer
                    .Add<AddVStep>((step, env, recurse) =>
                    {
                        if (threadLocal.Value == step)
                            return default;

                        threadLocal.Value = step;

                        try
                        {
                            return new[]
                            {
                                recurse.TransformTo<object>().From(step, env),
                                recurse.TransformTo<object>().From(new PropertyStep.ByKeyStep("PartitionKey", "PartitionKey"), env)
                            };
                        }
                        finally
                        {
                            threadLocal.Value = null;
                        }
                    }));
        }
    }
}
