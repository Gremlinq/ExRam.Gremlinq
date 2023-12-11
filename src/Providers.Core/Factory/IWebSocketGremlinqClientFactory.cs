using System.Buffers;
using System.Net.WebSockets;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IWebSocketGremlinqClientFactory : IGremlinqClientFactory
    {
        IWebSocketGremlinqClientFactory WithMessageBufferFactory<TBuffer>(IMessageBufferFactory<TBuffer> factory)
            where TBuffer : IMessageBuffer;

        IWebSocketGremlinqClientFactory ConfigureUri(Func<Uri, Uri> transformation);

        IWebSocketGremlinqClientFactory ConfigureUsername(Func<string?, string?> transformation);

        IWebSocketGremlinqClientFactory ConfigurePassword(Func<string?, string?> transformation);

        IWebSocketGremlinqClientFactory ConfigureOptions(Action<ClientWebSocketOptions> configuration);
    }
}
