using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public abstract class ExecutingTestFixture : GremlinqTestFixture
    {
        protected ExecutingTestFixture(IGremlinQuerySource source) : base(source
            .ConfigureEnvironment(env => env
                .UseNewtonsoftJson()
                .ConfigureDeserializer(d => d
                    .Add(ConverterFactory
                        .Create<JToken, JToken>((token, env, recurse) => token)))))
        {
        }
    }
}
