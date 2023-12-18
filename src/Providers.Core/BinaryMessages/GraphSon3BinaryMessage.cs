using System.Buffers;

using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct GraphSon3BinaryMessage : IMemoryOwner<byte>
    {
        private readonly IMemoryOwner<byte>? _owner;

        public GraphSon3BinaryMessage(IMemoryOwner<byte> owner)
        {
            _owner = owner;
        }

        public void Dispose() => _owner?.Dispose();

        public Memory<byte> Memory => _owner?.Memory ?? throw ExceptionHelper.UninitializedStruct();
    }
}
