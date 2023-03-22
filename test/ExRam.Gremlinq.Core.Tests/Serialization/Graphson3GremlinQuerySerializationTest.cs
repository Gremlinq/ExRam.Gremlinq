using ExRam.Gremlinq.Core.Execution;
using Gremlin.Net.Structure.IO.GraphSON;
using static ExRam.Gremlinq.Core.GremlinQuerySource;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3GremlinQuerySerializationTest : QuerySerializationTest, IClassFixture<Graphson3GremlinQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            private static readonly GraphSON3Writer Writer = new();

            public Fixture() : base(g
                .ConfigureEnvironment(_ => _
                    .UseExecutor(GremlinQueryExecutor.Identity
                        .TransformResult(result => result
                            .OfType<BytecodeGremlinQuery>()
                            .Select(query => Writer.WriteObject(query.Bytecode))))))
            {
            }
        }

        public Graphson3GremlinQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
