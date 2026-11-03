using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class DefaultDebugGremlinQuerySerializationTests : DefaultDebugGremlinQuerySerializationTestsBase, IClassFixture<GremlinqFixture>, ISourceFileNameProvider<DefaultDebugGremlinQuerySerializationTests>
    {
        public DefaultDebugGremlinQuerySerializationTests(GremlinqFixture fixture) : base(fixture, new DebugVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
