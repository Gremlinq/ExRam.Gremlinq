using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using static ExRam.Gremlinq.Core.GremlinQuerySource;
using static ExRam.Gremlinq.Core.Tests.SerializationTestsBase;

namespace ExRam.Gremlinq.Core.Tests.Fixtures
{
    public sealed class Graphson2StringFixture : SerializationTestsFixture<string>
    {
        private static readonly GraphSON2Writer Writer = new();

        public Graphson2StringFixture() : base(g
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
}
