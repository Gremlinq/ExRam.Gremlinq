using System.Buffers;
using System.Net.WebSockets;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class ClientWebSocketExtensions
    {
        public readonly struct SlicedMemoryOwner : IMemoryOwner<byte>
        {
            private readonly IMemoryOwner<byte>? _owner;

            public SlicedMemoryOwner(IMemoryOwner<byte> owner, int length)
            {
                _owner = owner;
                Memory = _owner.Memory[..length];
            }

            public void Dispose()
            {
                _owner?.Dispose();
            }

            public Memory<byte> Memory { get; }
        }

        public static async Task<SlicedMemoryOwner> ReceiveAsync(this ClientWebSocket client, CancellationToken ct)
        {
            var read = 0;
            var bytes = MemoryPool<byte>.Shared.Rent(2048);

            while (true)
            {
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

