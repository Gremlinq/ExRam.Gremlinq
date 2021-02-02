using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QueryDeserializationTest : QueryExecutionTest
    {
        [ThreadStatic]
        private static Context? XUnitContext;

        protected QueryDeserializationTest(ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            GremlinQuerySource.g
                .ConfigureEnvironment(env => env
                    .UseExecutor(GremlinQueryExecutor.Create((_, _) =>
                    {
                        if (XUnitContext is { } context)
                        {
                            var prefix = context.ClassName.Substring(0, context.ClassName.Length - "DeserializationTests".Length);

                            try
                            {
                                var jArray = JsonConvert.DeserializeObject<JArray>(
                                    File.ReadAllText(System.IO.Path.Combine(context.SourceDirectory, prefix + "IntegrationTests." + XUnitContext.MethodName + ".verified.json")));

                                return jArray.Count == 1
                                    ? new[] { jArray[0] }.ToAsyncEnumerable()
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
                        }

                        throw new InvalidOperationException();
                    }))
                    .UseDeserializer(GremlinQueryExecutionResultDeserializer.FromJToken)),
            testOutputHelper,
            callerFilePath)
        {
            XUnitContext = Context;
        }
    }
}
