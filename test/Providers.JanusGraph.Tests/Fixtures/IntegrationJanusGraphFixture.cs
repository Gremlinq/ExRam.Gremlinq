using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.Core;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class IntegrationJanusGraphFixture : GremlinqTestFixture
    {
        public IntegrationJanusGraphFixture() : base(Gremlinq.Core.GremlinQuerySource.g
            .UseJanusGraph(builder => builder
                .At(new Uri("ws://localhost:8183"))
                .UseNewtonsoftJson())
            .ConfigureEnvironment(env => env
                .ConfigureDeserializer(d => d
                    .Add(ConverterFactory
                        .Create<JToken, JToken>((token, env, recurse) => token))))
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults())))
        {
        }
    }
}
