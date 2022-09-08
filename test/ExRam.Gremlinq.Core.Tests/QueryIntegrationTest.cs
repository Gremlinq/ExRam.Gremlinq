using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QueryIntegrationTest : QueryExecutionTest
    {
        public abstract class Fixture : GremlinqTestFixture
        {
            protected Fixture(IGremlinQuerySource source) : base(source
                .ConfigureEnvironment(env => env
                    .ConfigureExecutor(_ => _
                        .TransformResult(enumerable => enumerable
                            .Catch<object, Exception>(ex => AsyncEnumerableEx
                                .Return<object>(new JObject()
                                {
                                    {
                                        "serverException",
                                        new JObject
                                        {
                                            { "type", ex.GetType().Name },
                                            { "message", ex.Message }
                                        }
                                    }
                                }))))
                    .UseDeserializer(GremlinQueryExecutionResultDeserializer.Default)))
            {
            }
        }

        protected QueryIntegrationTest(Fixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<JToken>());
    }
}
