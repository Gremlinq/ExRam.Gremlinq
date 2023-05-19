using Gremlin.Net.Structure.IO.GraphSON;
using static ExRam.Gremlinq.Core.GremlinQuerySource;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3GremlinQuerySerializationTest : SerializationTestsBase<string>, IClassFixture<Graphson3GremlinQuerySerializationTest.Graphson3Fixture>
    {
        public sealed class Graphson3Fixture : GremlinqTestFixture
        {
            private static readonly GraphSON3Writer Writer = new();

            public Graphson3Fixture() : base(g
                .ConfigureEnvironment(_ => _
                    .ConfigureSerializer(ser => ser
                        .Add(ConverterFactory
                            .Create<IGremlinQueryBase, string>((query, env, recurse) => Writer
                                .WriteObject(recurse
                                    .TransformTo<Bytecode>()
                                    .From(query, env)))))))
            {
            }
        }

        public Graphson3GremlinQuerySerializationTest(Graphson3Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
