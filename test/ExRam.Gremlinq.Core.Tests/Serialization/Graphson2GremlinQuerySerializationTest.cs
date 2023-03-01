using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Structure.IO.GraphSON;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2GremlinQuerySerializationTest : QuerySerializationTest, IClassFixture<Graphson2GremlinQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .ConfigureEnvironment(_ => _
                    .ConfigureSerializer(_ => _
                        .Select(obj => obj is BytecodeGremlinQuery byteCodeQuery
                            ? new GraphSONGremlinQuery(byteCodeQuery.Id, Writer.WriteObject(byteCodeQuery.Bytecode))
                            : obj))
                    .UseExecutor(GremlinQueryExecutor.Identity)))
            {
            }
        }

        private static readonly GraphSON2Writer Writer = new();

        public Graphson2GremlinQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
