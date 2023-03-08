using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;

using Gremlin.Net.Process.Traversal;

using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    internal static class CosmosDbTestExtensions
    {
        public static IGremlinQueryEnvironment AddFakePartitionKey(this IGremlinQueryEnvironment env)
        {
            var threadLocal = new ThreadLocal<Stack<AddVStep>?>();

            return env
                .ConfigureSerializer(serializer => serializer
                    .Add(Create<AddVStep, Instruction[]>((step, env, recurse) =>
                    {
                        var stack = threadLocal.Value is { } presentStack
                            ? presentStack
                            : threadLocal.Value = new Stack<AddVStep>();

                        if (stack.TryPeek(out var result) && result == step)
                            return default;

                        stack.Push(step);

                        try
                        {
                            return new[]
                            {
                                recurse.TransformTo<Instruction>().From(step, env),
                                recurse.TransformTo<Instruction>().From(new PropertyStep.ByKeyStep("PartitionKey", "PartitionKey"), env)
                            };
                        }
                        finally
                        {
                            stack.Pop();
                        }
                    })));
        }
    }
}
