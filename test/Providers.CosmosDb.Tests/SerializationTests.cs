using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class SerializationTests : QueryExecutionTest, IClassFixture<CosmosDbFixture>
    {
        public SerializationTests(CosmosDbFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<GroovyGremlinQuery>(),
            testOutputHelper)
        {

        }

        [Fact]
        public async Task CosmosDbKey()
        {
            await _g
                .V<Person>(new CosmosDbKey("pk", "id"))
                .Verify();
        }

        [Fact]
        public async Task Inlined_CosmosDbKey()
        {
            await _g
                .V<Person>(new CosmosDbKey("pk", "id"))
                .Verify();
        }

        [Fact]
        public async Task CosmosDbKey_with_null_partitionKey()
        {
            await _g
                .V<Person>(new CosmosDbKey("id"))
                .Verify();
        }

        [Fact]
        public async Task Mixed_StringKey_CosmosDbKey()
        {
            await _g
                .V<Person>(new CosmosDbKey("pk", "id"), "id2")
                .Verify();
        }

        [Fact]
        public async Task Mixed_StringKey_CosmosDbKey_with_null_partitionKey()
        {
            await _g
                .V<Person>(new CosmosDbKey("id"), "id2")
                .Verify();
        }
    }
}
