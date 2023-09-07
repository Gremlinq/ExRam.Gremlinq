using Gremlin.Net.Structure.IO.GraphSON;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;
using System.Runtime.CompilerServices;
using Gremlin.Net.Driver.Messages;

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

            return this
                .InnerVerify(_writer
                    .WriteObject(env.Serializer
                        .TransformTo<RequestMessage>()
                        .From(query, env))
                .FormatJson())
                .ScrubGuids();
        }
    }
}
