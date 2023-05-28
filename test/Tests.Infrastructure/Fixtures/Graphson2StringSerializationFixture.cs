using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public sealed class Graphson2StringSerializationFixture : GremlinqFixture
    {
        private static readonly GraphSON2Writer Writer = new();

        public Graphson2StringSerializationFixture() : base(g
            .ConfigureEnvironment(_ => _
                .ConfigureSerializer(ser => ser
                    .Add(ConverterFactory
                        .Create<IGremlinQueryBase, string>((query, env, recurse) => Writer
                            .WriteObject(recurse
                                .TransformTo<Bytecode>()
                                .From(query, env))
                            .FormatJson())))))
        {
        }
    }
}
