using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public abstract class IntegrationTestFixture : GremlinqTestFixture
    {
        protected IntegrationTestFixture(IGremlinQuerySource source) : base(source
            .ConfigureEnvironment(env => env
                .UseNewtonsoftJson()
                .ConfigureDeserializer(d => d
                    .Add(ConverterFactory
                        .Create<JToken, JToken>((token, env, recurse) => token)))))
        {
        }

        public override async Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var serialized = JsonConvert.SerializeObject(
                await query
                    .Cast<JToken>()
                    .ToArrayAsync(),
                Formatting.Indented);

            var scrubbed = GremlinqTestBase.Current
                .Scrubbers()
                .Aggregate(serialized, (s, func) => func(s));

            await GremlinqTestBase.Current.Verify(scrubbed);
        }
    }
}
