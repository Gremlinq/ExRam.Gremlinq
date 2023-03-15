using ExRam.Gremlinq.Core.Execution;
using Gremlin.Net.Process.Traversal;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Structure.IO.GraphSON;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3GremlinQuerySerializationTest : QuerySerializationTest, IClassFixture<Graphson3GremlinQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            private static readonly GraphSON3Writer Writer = new();

            public Fixture() : base(g
                .ConfigureEnvironment(_ => _
                    .ConfigureSerializer(_ => _
                        .Add(ConverterFactory
                            .Create<Bytecode, GraphSONGremlinQuery>((bytecode, env, recurse) => new GraphSONGremlinQuery(Writer.WriteObject(bytecode)))))
                    .UseExecutor(GremlinQueryExecutor.Identity)))
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
