using System.Buffers;

namespace ExRam.Gremlinq.Providers.Core
{
    // The bytes of one received WebSocket message, in an array rented from the shared pool. Every
    // message gets an instance of its own and only the arrays are reused: a binary message compares
    // by the identity of its owner, and Newtonsoft's deferral caches the last deserialization by it.
    internal sealed class ReceivedMessage : IMemoryOwner<byte>
    {
        private const int InitialLength = 8192;

        private byte[]? _array;
        private int _length;

        private ReceivedMessage(byte[] array)
        {
            _array = array;
        }

        public Memory<byte> Memory => _array is { } array
            ? array.AsMemory(0, _length)
            : throw new ObjectDisposedException(nameof(ReceivedMessage));

        // The free part of the array, grown when it is full.
        public Memory<byte> GetFreeMemory()
        {
            if (_array is not { } array)
                throw new ObjectDisposedException(nameof(ReceivedMessage));

            if (_length == array.Length)
            {
                var newArray = ArrayPool<byte>.Shared.Rent(array.Length * 2);

                array.CopyTo(newArray, 0);
                ArrayPool<byte>.Shared.Return(array);

                _array = array = newArray;
            }

            return array.AsMemory(_length);
        }

        public void Advance(int count) => _length += count;

        public void Dispose()
        {
            if (_array is { } array)
            {
                _array = null;

                ArrayPool<byte>.Shared.Return(array);
            }
        }

        public static ReceivedMessage Rent() => new(ArrayPool<byte>.Shared.Rent(InitialLength));
    }
}
