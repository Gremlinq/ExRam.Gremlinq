using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class DeserializingGremlinqVerifier<TIntegrationTest> : GremlinQueryVerifier
        where TIntegrationTest : GremlinqTestBase
    {
        private readonly Context _context;

        public DeserializingGremlinqVerifier(ITestOutputHelper testOutputHelper, Func<SettingsTask, SettingsTask>? settingsTaskModifier = null, [CallerFilePath] string sourceFile = "") : base(settingsTaskModifier, sourceFile)
        {
            _context = XunitContext.Register(testOutputHelper, sourceFile);
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var environment = query.AsAdmin().Environment;

            if (JsonConvert.DeserializeObject<JArray>(File.ReadAllText(Path.Combine(_context.SourceDirectory, typeof(TIntegrationTest).Name + "." + _context.MethodName + ".verified.txt"))) is { } jArray)
            {
                return base
                    .InnerVerify(jArray
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
            else
                throw new InvalidOperationException();
        }
    }
}
