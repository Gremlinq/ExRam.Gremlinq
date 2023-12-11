using System.Buffers;

using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct GraphSon3MessageBuffer : IMessageBuffer
    {
        private readonly IMemoryOwner<byte>? _owner;

        public GraphSon3MessageBuffer(IMemoryOwner<byte> owner)
        {
            _owner = owner;
        }

        public void Dispose() => _owner?.Dispose();

        public Memory<byte> Memory => _owner?.Memory ?? throw ExceptionHelper.UninitializedStruct();
    }
}
