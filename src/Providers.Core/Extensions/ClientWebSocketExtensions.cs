using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class ClientWebSocketExtensions
    {
        [AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder<>))]
        public static async ValueTask<ReceivedMessage> ReceiveAsync(this ClientWebSocket client, CancellationToken ct)
        {
            var message = ReceivedMessage.Rent();

            try
            {
                ValueWebSocketReceiveResult result;

                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await client.ReceiveAsync(message.GetFreeMemory(), ct).ConfigureAwait(false);

                    if (result.MessageType == WebSocketMessageType.Close)
                        throw new ObjectDisposedException(client.GetType().Name);

                    message.Advance(result.Count);
                }
                while (!result.EndOfMessage);

                return message;
            }
            catch
            {
                message.Dispose();

                throw;
            }
        }
    }
}
