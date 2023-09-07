using Gremlin.Net.Structure.IO.GraphSON;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using ExRam.Gremlinq.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class GraphSonStringSerializationFixture : GremlinqFixture
    {
        protected GraphSonStringSerializationFixture(GraphSONWriter writer) : base(g
            .ConfigureEnvironment(_ => _
                .ConfigureSerializer(ser => ser
                    .Add(ConverterFactory
                        .Create<IGremlinQueryBase, string>((query, env, recurse) => writer
                            .WriteObject(recurse
                                .TransformTo<Bytecode>()
                                .From(query, env))
                            .FormatJson())))))
        {
        }
    }
}
