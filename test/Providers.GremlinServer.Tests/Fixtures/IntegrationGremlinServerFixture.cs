using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.Core;
using Newtonsoft.Json.Linq;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class IntegrationGremlinServerFixture : GremlinqFixture
    {
        public IntegrationGremlinServerFixture() : base(g
            .UseGremlinServer(builder => builder
                .AtLocalhost()
                .UseNewtonsoftJson())
            .ConfigureEnvironment(env => env
                .ConfigureDeserializer(d => d
                    .Add(ConverterFactory
                        .Create<JToken, JToken>((token, env, recurse) => token)))))
        {
        }
    }
}
