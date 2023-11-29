using System.Buffers;
using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct SlicedMemoryOwner : IMemoryOwner<byte>
    {
        private readonly int _length;
        private readonly IMemoryOwner<byte>? _owner;

        public SlicedMemoryOwner(IMemoryOwner<byte> owner, int length)
        {
            _owner = owner;
            _length = length;
        }

        public void Dispose()
        {
            _owner?.Dispose();
        }

        public Memory<byte> Memory { get => _owner?.Memory[.._length] ?? throw UninitializedStruct(); }
    }
}

