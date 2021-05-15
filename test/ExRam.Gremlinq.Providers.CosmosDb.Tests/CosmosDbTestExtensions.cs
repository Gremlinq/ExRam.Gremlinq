using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    internal static class CosmosDbTestExtensions
    {
        public static IGremlinQueryEnvironment AddFakePartitionKey(this IGremlinQueryEnvironment env)
        {
            return env
                .ConfigureAddStepHandler(stepHandler => stepHandler
                    .Override<AddVStep>((steps, step, env, overridden, recurse) => overridden(steps, step, env, recurse)
                        .Push(new PropertyStep.ByKeyStep("PartitionKey", "PartitionKey"))));
        }
    }
}
