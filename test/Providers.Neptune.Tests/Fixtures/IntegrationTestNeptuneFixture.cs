using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.Core;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class IntegrationTestNeptuneFixture : GremlinqFixture
    {
        public IntegrationTestNeptuneFixture() : base(Gremlinq.Core.GremlinQuerySource.g
            .UseNeptune(builder => builder
                .AtLocalhost())
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
