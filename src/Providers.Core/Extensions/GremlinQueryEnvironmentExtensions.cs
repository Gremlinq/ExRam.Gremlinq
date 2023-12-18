using CommunityToolkit.HighPerformance.Buffers;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;

using System.Buffers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private static readonly byte[] GraphSon2Header = GetHeader("application/vnd.gremlin-v2.0+json");
        private static readonly byte[] GraphSon3Header = GetHeader("application/vnd.gremlin-v3.0+json");
        private static readonly JsonSerializerOptions JsonOptions = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

        public static IGremlinQueryEnvironment AddGraphSonSupport(this IGremlinQueryEnvironment environment) => environment
            .AddGraphSonSupport(new GraphSON2Writer(), GraphSon2Header, owner => new GraphSon2BinaryMessage(owner))
            .AddGraphSonSupport(new GraphSON3Writer(), GraphSon3Header, owner => new GraphSon3BinaryMessage(owner));

        private static byte[] GetHeader(string mimeType) => mimeType.Length <= 255
            ? Encoding.UTF8.GetBytes($"{(char)mimeType.Length}{mimeType}")
            : throw new ArgumentException();

        private static IGremlinQueryEnvironment AddGraphSonSupport<TBuffer>(this IGremlinQueryEnvironment environment, GraphSONWriter writer, byte[] header, Func<IMemoryOwner<byte>, TBuffer> bufferFactory)
            where TBuffer : struct, IMemoryOwner<byte>
        {
            return environment
                .ConfigureSerializer(serializer => serializer
                    .Add(Create<RequestMessage, TBuffer>((message, _, _, _) =>
                    {
                        var bufferWriter = new ArrayPoolBufferWriter<byte>();

                        bufferWriter.Write(header.AsSpan());

                        try
                        {
                            JsonSerializer.Serialize(new Utf8JsonWriter(bufferWriter), (object)writer.ToDict(message), JsonOptions);
                        }
                        catch
                        {
                            using (bufferWriter)
                            {
                                throw;
                            }
                        }

                        return bufferFactory(bufferWriter);
                    })))
                .ConfigureDeserializer(deserializer => deserializer
                    .Add(Create<IMemoryOwner<byte>, TBuffer>((owner, _, _, _) => bufferFactory(owner))));
        }
    }
}
