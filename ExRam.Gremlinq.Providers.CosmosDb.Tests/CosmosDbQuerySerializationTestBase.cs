using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public abstract class CosmosDbQuerySerializationTestBase : QuerySerializationTest
    {
        protected CosmosDbQuerySerializationTestBase(IGremlinQuerySource g, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(g, testOutputHelper, callerFilePath)
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
