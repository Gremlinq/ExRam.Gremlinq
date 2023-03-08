using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    internal static class CosmosDbTestExtensions
    {
        public static IGremlinQueryEnvironment AddFakePartitionKey(this IGremlinQueryEnvironment env) => env
            .ConfigureSerializer(serializer => serializer
                .Add(Create<AddVStep, Instruction[]>((step, env, recurse) => new[]
                {
                    recurse.TransformTo<Instruction>().From(step, env),
                    recurse.TransformTo<Instruction>().From(new PropertyStep.ByKeyStep("PartitionKey", "PartitionKey"), env)
                })));
    }
}
