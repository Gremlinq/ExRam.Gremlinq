using System.Buffers;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class MessageBufferFactory
    {
        private sealed class GraphSon2MessageBufferFactory : IMessageBufferFactory<GraphSon2MessageBuffer>
        {
            public GraphSon2MessageBufferFactory()
            {

            }

            GraphSon2MessageBuffer IMessageBufferFactory<GraphSon2MessageBuffer>.Create(IMemoryOwner<byte> message) => new(message);
        }

        private sealed class GraphSon3MessageBufferFactory : IMessageBufferFactory<GraphSon3MessageBuffer>
        {
            public GraphSon3MessageBufferFactory()
            {

            }

            GraphSon3MessageBuffer IMessageBufferFactory<GraphSon3MessageBuffer>.Create(IMemoryOwner<byte> message) => new(message);
        }

        public static readonly IMessageBufferFactory<GraphSon2MessageBuffer> GraphSon2 = new GraphSon2MessageBufferFactory();
        public static readonly IMessageBufferFactory<GraphSon3MessageBuffer> GraphSon3 = new GraphSon3MessageBufferFactory();
    }
}
