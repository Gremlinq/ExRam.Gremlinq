using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbGroovySerializationTest : GroovySerializationTest
    {
        public CosmosDbGroovySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                    .UseCosmosDb(builder => builder
                        .At(new Uri("wss://localhost"), "database", "graph")
                        .AuthenticateBy("authKey"))
                    .ConfigureOptions(options => options
                        .SetItem(GremlinqOption.DontAddElementProjectionSteps, false))),
            testOutputHelper)
        {

        }

        [Fact]
        public void CosmosDbKey()
        {
            _g
                .V<Person>(new CosmosDbKey("pk", "id"))
                .VerifyQuery(this);
        }

        [Fact]
        public void CosmosDbKey_with_null_partitionKey()
        {
            _g
                .V<Person>(new CosmosDbKey("id"))
                .VerifyQuery(this);
        }

        [Fact]
        public void Mixed_StringKey_CosmosDbKey()
        {
            _g
                .V<Person>(new CosmosDbKey("pk", "id"), "id2")
                .VerifyQuery(this);
        }

        [Fact]
        public void Mixed_StringKey_CosmosDbKey_with_null_partitionKey()
        {
            _g
                .V<Person>(new CosmosDbKey("id"), "id2")
                .VerifyQuery(this);
        }
    }
}
