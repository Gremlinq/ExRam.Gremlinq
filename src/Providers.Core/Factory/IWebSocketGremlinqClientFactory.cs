using System.Buffers;
using System.Net.WebSockets;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// A client factory that creates WebSocket-based Gremlin clients.
    /// </summary>
    public interface IWebSocketGremlinqClientFactory : IGremlinqClientFactory<IWebSocketGremlinqClientFactory>
    {
        /// <summary>
        /// Configures the binary message type used for WebSocket communication.
        /// </summary>
        /// <typeparam name="TBinaryMessage">The binary message type that implements <see cref="IMemoryOwner{T}"/>.</typeparam>
        IWebSocketGremlinqClientFactory WithBinaryMessage<TBinaryMessage>()
            where TBinaryMessage : IMemoryOwner<byte>;

        /// <summary>
        /// Configures the server URI by applying a transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the current URI.</param>
        IWebSocketGremlinqClientFactory ConfigureUri(Func<Uri, Uri> transformation);

        /// <summary>
        /// Configures the <see cref="ClientWebSocket"/> factory by applying a transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the current WebSocket factory function.</param>
        IWebSocketGremlinqClientFactory ConfigureClientWebSocketFactory(Func<Func<ClientWebSocket>, Func<ClientWebSocket>> transformation);

        /// <summary>
        /// Configures the authentication message factory by applying a transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the current authentication message factory.</param>
        IWebSocketGremlinqClientFactory ConfigureAuthentication(Func<Func<IReadOnlyDictionary<string, object>, RequestMessage>, Func<IReadOnlyDictionary<string, object>, RequestMessage>> transformation);
    }
}
