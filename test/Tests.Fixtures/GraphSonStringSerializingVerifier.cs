using Gremlin.Net.Structure.IO.GraphSON;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class GraphSonStringSerializingVerifier : GremlinQueryVerifier
    {
        private readonly GraphSONWriter _writer;

        public GraphSonStringSerializingVerifier(GraphSONWriter writer, [CallerFilePath] string sourceFile = "") : base(sourceFile)
        {
            _writer = writer;
        }

        public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var env = query.AsAdmin().Environment;

            return InnerVerify(_writer
                .WriteObject(env.Serializer
                    .TransformTo<Bytecode>()
                    .From(query, env))
                .FormatJson());
        }
    }
}
