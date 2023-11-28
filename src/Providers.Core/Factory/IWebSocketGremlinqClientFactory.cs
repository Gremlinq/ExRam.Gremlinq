using System.Net.WebSockets;

using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IWebSocketGremlinqClientFactory : IGremlinqClientFactory
    {
        IWebSocketGremlinqClientFactory ConfigureUri(Func<Uri, Uri> transformation);

        IWebSocketGremlinqClientFactory ConfigureUsername(Func<string?, string?> transformation);

        IWebSocketGremlinqClientFactory ConfigurePassword(Func<string?, string?> transformation);

        IWebSocketGremlinqClientFactory ConfigureOptions(Action<ClientWebSocketOptions> configuration);
    }
}
