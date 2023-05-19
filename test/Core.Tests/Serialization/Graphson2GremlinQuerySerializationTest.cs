using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2GremlinQuerySerializationTest : SerializationTestsBase<string>, IClassFixture<Graphson2GremlinQuerySerializationTest.Graphson2Fixture>
    {
        public sealed class Graphson2Fixture : GremlinqTestFixture
        {
            private static readonly GraphSON2Writer Writer = new();

            public Graphson2Fixture() : base(g
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

        public Graphson2GremlinQuerySerializationTest(Graphson2Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
