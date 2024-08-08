using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class DeserializingGremlinqVerifier<TIntegrationTest> : GremlinQueryVerifier
        where TIntegrationTest : GremlinqTestBase
    {
        private readonly Context _context;

        public DeserializingGremlinqVerifier(ITestOutputHelper testOutputHelper, [CallerFilePath] string sourceFile = "") : base(sourceFile)
        {
            _context = XunitContext.Register(testOutputHelper, sourceFile);
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var environment = query.AsAdmin().Environment;

            var text = File.ReadAllText(Path.Combine(_context.SourceDirectory, typeof(TIntegrationTest).Name + "." + _context.MethodName + ".verified.txt"));

            return text.Contains("GremlinQueryExecutionException")
                ? InnerVerify(text)
                : InnerVerify(environment.Deserializer
                   .TransformTo<TElement[]>()
                   .From(JToken.Parse(text), environment));
        }

        protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
            .ModifySettingsTask(task)
            .DontScrubDateTimes()
            .DontScrubGuids()
            .DontIgnoreEmptyCollections();
    }
}
