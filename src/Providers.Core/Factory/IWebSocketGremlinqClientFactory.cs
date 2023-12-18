using System.Buffers;
using System.Net.WebSockets;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IWebSocketGremlinqClientFactory : IGremlinqClientFactory
    {
        IWebSocketGremlinqClientFactory WithBinaryMessage<TBinaryMessage>()
            where TBinaryMessage : IMemoryOwner<byte>;

        IWebSocketGremlinqClientFactory ConfigureUri(Func<Uri, Uri> transformation);

        IWebSocketGremlinqClientFactory ConfigureClientWebSocketFactory(Func<Func<ClientWebSocket>, Func<ClientWebSocket>> transformation);

        IWebSocketGremlinqClientFactory ConfigureAuthentication(Func<Func<IReadOnlyDictionary<string, object>, RequestMessage>, Func<IReadOnlyDictionary<string, object>, RequestMessage>> transformation);
    }
}
