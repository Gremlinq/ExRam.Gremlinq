using System.Buffers;
using System.Net.WebSockets;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class ClientWebSocketExtensions
    {
        public static async Task<SlicedMemoryOwner> ReceiveAsync(this ClientWebSocket client, CancellationToken ct)
        {
            var read = 0;
            var bytes = MemoryPool<byte>.Shared.Rent(2048);

            while (true)
            {
                ct.ThrowIfCancellationRequested();

                if (read == bytes.Memory.Length)
                    bytes = MemoryPool<byte>.Shared.Double(bytes);

                var result = await client.ReceiveAsync(bytes.Memory[read..], ct);

                read += result.Count;

                if (result.EndOfMessage)
                    break;
            }

            return new SlicedMemoryOwner(bytes, read);
        }
    }
}

