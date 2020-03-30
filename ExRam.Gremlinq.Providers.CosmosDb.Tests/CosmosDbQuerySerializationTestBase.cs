using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public abstract class CosmosDbQuerySerializationTestBase : QuerySerializationTest
    {
        protected CosmosDbQuerySerializationTestBase(IGremlinQuerySource g, ITestOutputHelper testOutputHelper) : base(g, testOutputHelper)
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
