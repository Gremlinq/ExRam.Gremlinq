using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class DeserializingGremlinqVerifier : GremlinQueryVerifier
    {
        private readonly Context _context;

        public DeserializingGremlinqVerifier(ITestOutputHelper testOutputHelper, [CallerFilePath] string sourceFile = "") : base(sourceFile)
        {
            _context = XunitContext.Register(testOutputHelper, sourceFile);
        }

        public override async Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var environment = query.AsAdmin().Environment;

            if (JsonConvert.DeserializeObject<JArray>(File.ReadAllText(Path.Combine(_context.SourceDirectory, "IntegrationTests" + "." + _context.MethodName + ".verified.txt"))) is { } jArray)
            {
                await base
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
        }
    }
}
