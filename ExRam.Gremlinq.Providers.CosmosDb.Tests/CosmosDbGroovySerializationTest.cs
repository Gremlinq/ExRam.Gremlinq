using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbGroovySerializationTestWithProjection : CosmosDbGroovySerializationTest
    {
        public CosmosDbGroovySerializationTestWithProjection(ITestOutputHelper testOutputHelper) : base(
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
    }

    public class CosmosDbGroovySerializationTestWithoutProjection : CosmosDbGroovySerializationTest
    {
        public CosmosDbGroovySerializationTestWithoutProjection(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                    .UseCosmosDb(builder => builder
                        .At(new Uri("wss://localhost"), "database", "graph")
                        .AuthenticateBy("authKey"))
                    .ConfigureOptions(options => options
                        .SetItem(GremlinqOption.DontAddElementProjectionSteps, true))),
            testOutputHelper)
        {

        }
    }

    public abstract class CosmosDbGroovySerializationTest : GroovySerializationTest
    {
        public CosmosDbGroovySerializationTest(IGremlinQuerySource g, ITestOutputHelper testOutputHelper) : base(g, testOutputHelper)
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
