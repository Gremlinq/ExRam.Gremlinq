using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Steps;

using Gremlin.Net.Process.Traversal;

using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests.Extensions
{
    internal static class GremlinQueryEnvironmentExtensions
    {
        public static IGremlinQueryEnvironment AddFakePartitionKey(this IGremlinQueryEnvironment env) => env;
            // .ConfigureSerializer(serializer => serializer
            //     .Add(Create<AddVStep, Instruction[]>((step, env, _, recurse) => new[]
            //     {
            //         recurse.TransformTo<Instruction>().From(step, env),
            //         recurse.TransformTo<Instruction>().From(new PropertyStep.ByKeyStep("PartitionKey", "PartitionKey"), env)
            //     })));
    }
}
