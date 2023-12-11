using CommunityToolkit.HighPerformance.Buffers;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Structure.IO.GraphSON;

using System.Buffers;
using System.Text;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class MessageBufferFactory
    {
        private sealed class GraphSon2MessageBufferFactory : GraphSonMessageBufferFactory, IMessageBufferFactory<GraphSon2MessageBuffer>
        {
            public GraphSon2MessageBufferFactory() : base(new GraphSON2Writer(), "application/vnd.gremlin-v2.0+json")
            {

            }

            GraphSon2MessageBuffer IMessageBufferFactory<GraphSon2MessageBuffer>.Create(IMemoryOwner<byte> message) => new(message);

            GraphSon2MessageBuffer IMessageBufferFactory<GraphSon2MessageBuffer>.Create(RequestMessage message) => new (Create(message));
        }

        private sealed class GraphSon3MessageBufferFactory : GraphSonMessageBufferFactory, IMessageBufferFactory<GraphSon3MessageBuffer>
        {
            public GraphSon3MessageBufferFactory() : base(new GraphSON3Writer(), "application/vnd.gremlin-v3.0+json")
            {

            }

            GraphSon3MessageBuffer IMessageBufferFactory<GraphSon3MessageBuffer>.Create(IMemoryOwner<byte> message) => new(message);

            GraphSon3MessageBuffer IMessageBufferFactory<GraphSon3MessageBuffer>.Create(RequestMessage message) => new (Create(message));
        }

        private abstract class GraphSonMessageBufferFactory
        {
            private readonly byte[] _mimeTypeBytes;
            private readonly GraphSONWriter _graphSONWriter;

            protected GraphSonMessageBufferFactory(GraphSONWriter graphSONWriter, string mimeType)
            {
                _graphSONWriter = graphSONWriter;
                _mimeTypeBytes = Encoding.UTF8.GetBytes($"{(char)mimeType.Length}{mimeType}");
            }

            protected IMemoryOwner<byte> Create(RequestMessage message)
            {
                var graphSONMessage = _graphSONWriter.WriteObject(message);
                var bytesNeeded = Encoding.UTF8.GetByteCount(graphSONMessage) + _mimeTypeBytes.Length;
                var memory = MemoryOwner<byte>.Allocate(bytesNeeded);

                _mimeTypeBytes
                    .AsSpan()
                    .CopyTo(memory.Memory.Span);

                Encoding.UTF8.GetBytes(graphSONMessage.AsSpan(), memory.Memory.Span[_mimeTypeBytes.Length..]);

                return memory;
            }
        }

        public static readonly IMessageBufferFactory<GraphSon2MessageBuffer> GraphSon2 = new GraphSon2MessageBufferFactory();
        public static readonly IMessageBufferFactory<GraphSon3MessageBuffer> GraphSon3 = new GraphSon3MessageBufferFactory();
    }
}
