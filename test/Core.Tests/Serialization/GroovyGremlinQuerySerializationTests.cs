using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class GroovyGremlinQuerySerializationTests : QueryExecutionTestsBase, IClassFixture<GroovyGremlinQuerySerializationFixture>
    {
        public GroovyGremlinQuerySerializationTests(GroovyGremlinQuerySerializationFixture fixture) : base(
            fixture,
            new SerializingVerifier<GroovyGremlinScript>())
        {
        }

        [Fact]
        public Task Variable_wrap() => _g
            .V()
            .Properties()
            .Properties(Enumerable
                .Range(1, 1000)
                .Select(x => x.ToString())
                .ToArray())
            .Verify();
    }
}
