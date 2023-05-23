using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Tests.Verifiers;
using ExRam.Gremlinq.Providers.Neptune.Tests.Fixtures;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class SerializationTests : QueryExecutionTest, IClassFixture<SimpleNeptuneFixture>
    {
        public SerializationTests(SimpleNeptuneFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<Bytecode>(),
            testOutputHelper)
        {
        }
    }
}
