using System.Buffers;
using System.Net.WebSockets;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IWebSocketGremlinqClientFactory : IGremlinqClientFactory
    {
        IWebSocketGremlinqClientFactory WithMessageBufferFactory<TBuffer>(IMessageBufferFactory<TBuffer> factory)
            where TBuffer : IMemoryOwner<byte>;

        IWebSocketGremlinqClientFactory ConfigureUri(Func<Uri, Uri> transformation);

        IWebSocketGremlinqClientFactory ConfigureAuthentication(Func<IReadOnlyDictionary<string, object>, RequestMessage> requestMessageFactory);

        IWebSocketGremlinqClientFactory ConfigureOptions(Action<ClientWebSocketOptions> configuration);
    }
}
