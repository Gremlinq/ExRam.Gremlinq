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
                    .ConfigureAddStepHandler(stepHandler => stepHandler
                        .Override<AddVStep>((steps, step, env, overridden, recurse) => overridden(steps, step, env, recurse)
                            .Push(new PropertyStep("PartitionKey", "PartitionKey"))))
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
                .Verify(this);
        }

        [Fact]
        public async Task CosmosDbKey_with_null_partitionKey()
        {
            await _g
                .V<Person>(new CosmosDbKey("id"))
                .Verify(this);
        }

        [Fact]
        public async Task Mixed_StringKey_CosmosDbKey()
        {
            await _g
                .V<Person>(new CosmosDbKey("pk", "id"), "id2")
                .Verify(this);
        }

        [Fact]
        public async Task Mixed_StringKey_CosmosDbKey_with_null_partitionKey()
        {
            await _g
                .V<Person>(new CosmosDbKey("id"), "id2")
                .Verify(this);
        }
    }
}
