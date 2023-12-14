using CommunityToolkit.HighPerformance.Buffers;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;

using System.Buffers;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class MessageBufferFactory
    {
        private sealed class GraphSon2MessageBufferFactory : GraphSonMessageBufferFactory, IMessageBufferFactory<GraphSon2MessageBuffer>
        {
            public GraphSon2MessageBufferFactory() : base(new GraphSON2Writer())
            {

            }

            GraphSon2MessageBuffer IMessageBufferFactory<GraphSon2MessageBuffer>.Create(IMemoryOwner<byte> message) => new(message);

            GraphSon2MessageBuffer IMessageBufferFactory<GraphSon2MessageBuffer>.Create(RequestMessage message) => new (Create(message));

            string IMessageBufferFactory<GraphSon2MessageBuffer>.MimeType => "application/vnd.gremlin-v2.0+json";
        }

        private sealed class GraphSon3MessageBufferFactory : GraphSonMessageBufferFactory, IMessageBufferFactory<GraphSon3MessageBuffer>
        {
            public GraphSon3MessageBufferFactory() : base(new GraphSON3Writer())
            {

            }

            GraphSon3MessageBuffer IMessageBufferFactory<GraphSon3MessageBuffer>.Create(IMemoryOwner<byte> message) => new(message);

            GraphSon3MessageBuffer IMessageBufferFactory<GraphSon3MessageBuffer>.Create(RequestMessage message) => new (Create(message));

            string IMessageBufferFactory<GraphSon3MessageBuffer>.MimeType => "application/vnd.gremlin-v3.0+json";
        }

        private abstract class GraphSonMessageBufferFactory
        {
            private readonly GraphSONWriter _graphSONWriter;

            private static readonly JsonSerializerOptions JsonOptions = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

            protected GraphSonMessageBufferFactory(GraphSONWriter graphSONWriter)
            {
                _graphSONWriter = graphSONWriter;
            }

            protected IMemoryOwner<byte> Create(RequestMessage message)
            {
                var bufferWriter = new ArrayPoolBufferWriter<byte>();

                try
                {
                    JsonSerializer.Serialize(new Utf8JsonWriter(bufferWriter), (object)_graphSONWriter.ToDict(message), JsonOptions);
                }
                catch
                {
                    using (bufferWriter)
                    {
                        throw;
                    }
                }

                return bufferWriter;
            }
        }

        public static readonly IMessageBufferFactory<GraphSon2MessageBuffer> GraphSon2 = new GraphSon2MessageBufferFactory();
        public static readonly IMessageBufferFactory<GraphSon3MessageBuffer> GraphSon3 = new GraphSon3MessageBufferFactory();
    }
}
