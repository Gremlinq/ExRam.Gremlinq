using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class CosmosDbQuerySerializationTest : QuerySerializationTest
    {
        public CosmosDbQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinQuerySource.g
                .ConfigureEnvironment(env => env
                    .AddFakePartitionKey()
                    .UseCosmosDb(builder => builder
                        .At(new Uri("wss://localhost"), "database", "graph")
                        .AuthenticateBy("authKey"))),
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
                .ConfigureEnvironment(e => e
                    .ConfigureSerializer(s => s
                        .ToGroovy(GroovyFormatting.AllowInlining)))
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
