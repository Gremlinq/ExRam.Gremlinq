using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Structure.IO.GraphSON;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2GremlinQuerySerializationTest : SerializationTestsBase<string>, IClassFixture<Graphson2GremlinQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            private static readonly GraphSON2Writer Writer = new();

            public Fixture() : base(g
                .ConfigureEnvironment(_ => _
                    .ConfigureSerializer(ser => ser
                        .Add(ConverterFactory
                            .Create<BytecodeGremlinQuery, string>((query, env, recurse) => Writer.WriteObject(query.Bytecode))))))
            {
            }
        }

        public Graphson2GremlinQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
