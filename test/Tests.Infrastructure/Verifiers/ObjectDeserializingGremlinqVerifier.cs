using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class ObjectDeserializingGremlinqVerifier<TIntegrationTest> : DeserializingGremlinqVerifier<TIntegrationTest>
        where TIntegrationTest : GremlinqTestBase
    {
        public ObjectDeserializingGremlinqVerifier(ITestOutputHelper testOutputHelper, Func<SettingsTask, SettingsTask>? settingsTaskModifier = null, [CallerFilePath] string sourceFile = "") : base(testOutputHelper, settingsTaskModifier, sourceFile)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<object>());
    }
}
