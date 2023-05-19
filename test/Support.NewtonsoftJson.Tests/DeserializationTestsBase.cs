using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public abstract class DeserializationTestsBase : QueryExecutionTest
    {
        public abstract class DeserializationTestFixture : GremlinqTestFixture
        {
            protected DeserializationTestFixture(IGremlinQuerySource source) : base(source)
            {
            }

            public override async Task Verify<TElement>(IGremlinQueryBase<TElement> query)
            {
                var context = XunitContext.Context;
                var environment = query.AsAdmin().Environment;

                try
                {
                    if (JsonConvert.DeserializeObject<JArray>(File.ReadAllText(System.IO.Path.Combine(context.SourceDirectory, "IntegrationTests" + "." + context.MethodName + "." + Namer.RuntimeAndVersion + ".verified.txt"))) is { } jArray)
                    {
                        await Current
                            .Verify(jArray
                                .Where(obj => !(obj is JObject jObject && jObject.ContainsKey("serverException")))
                                .Select(token => environment
                                    .Deserializer
                                    .TransformTo<TElement>()
                                    .From(token, environment))
                                .ToArray())
                            .DontScrubDateTimes()
                            .DontScrubGuids()
                            .DontIgnoreEmptyCollections();
                    }
                }
                catch (IOException)
                {

                }
            }
        }

        protected DeserializationTestsBase(GremlinqTestFixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }
    }
}
