using System.Net.WebSockets;

using CommunityToolkit.HighPerformance.Buffers;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class ClientWebSocketExtensions
    {
        public static async Task<MemoryOwner<byte>> ReceiveAsync(this ClientWebSocket client, CancellationToken ct)
        {
            var read = 0;
            var bytes = MemoryOwner<byte>.Allocate(2048);

            try
            {
                ValueWebSocketReceiveResult result;

                do
                {
                    ct.ThrowIfCancellationRequested();

                    if (read == bytes.Memory.Length)
                        bytes = bytes.Double();

                    result = await client.ReceiveAsync(bytes.Memory[read..], ct);

                    if (result.MessageType == WebSocketMessageType.Close)
                        throw new ObjectDisposedException(client.GetType().Name);

                    read += result.Count;
                }
                while (!result.EndOfMessage);

                return bytes[..read];
            }
            catch
            {
                bytes.Dispose();

                throw;
            }
        }
    }
}

