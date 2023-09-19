using Gremlin.Net.Structure.IO.GraphSON;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;
using System.Runtime.CompilerServices;
using Gremlin.Net.Driver.Messages;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class GraphSonStringSerializingVerifier : GremlinQueryVerifier
    {
        private readonly GraphSONWriter _writer;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        public GraphSonStringSerializingVerifier(GraphSONWriter writer, Func<SettingsTask, SettingsTask>? settingsTaskModifier = null, [CallerFilePath] string sourceFile = "") : base(settingsTaskModifier, sourceFile)
        {
            _writer = writer;
        }

        public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var env = query.AsAdmin().Environment;

            return this
                .InnerVerify(JsonSerializer
                    .Serialize((object)_writer
                        .ToDict(env.Serializer
                            .TransformTo<RequestMessage>()
                            .From(query, env)), JsonOptions))
                .ScrubGuids();
        }
    }
}
