using Gremlin.Net.Structure.IO.GraphSON;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests.Fixtures
{
    public sealed class GraphSon3StringSerializationFixture : GremlinqFixture
    {
        private static readonly GraphSON3Writer Writer = new();

        public GraphSon3StringSerializationFixture() : base(g
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
