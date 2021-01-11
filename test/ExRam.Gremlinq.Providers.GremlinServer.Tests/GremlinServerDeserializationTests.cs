using System;
using System.IO;
using System.Linq;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class GremlinServerDeserializationTests : QueryExecutionTest
    {
        [ThreadStatic]
        private static Context XUnitContext;

        public GremlinServerDeserializationTests(ITestOutputHelper testOutputHelper) : base(
            GremlinQuerySource.g
                .ConfigureEnvironment(env => env
                    .UseExecutor(GremlinQueryExecutor.Create((_, _) =>
                    {
                        try
                        {
                            var jArray = JsonConvert.DeserializeObject<JArray>(
                                File.ReadAllText(System.IO.Path.Combine(XUnitContext.SourceDirectory, "GremlinServerIntegrationTests." + XUnitContext.MethodName + ".verified.json")));

                            return jArray.Count == 1
                                ? new[] {jArray[0]}.ToAsyncEnumerable()
                                : jArray.Count == 0
                                    ? AsyncEnumerable.Empty<object>()
                                    : throw new NotSupportedException();
                        }
                        catch (IOException)
                        {
                            #if !SKIPINTEGRATIONTESTS
                            throw;
                            #endif
                        }
                    }))
                    .UseDeserializer(GremlinQueryExecutionResultDeserializer.FromJToken)),
            testOutputHelper)
        {
            XUnitContext = Context;
        }
    }
}
