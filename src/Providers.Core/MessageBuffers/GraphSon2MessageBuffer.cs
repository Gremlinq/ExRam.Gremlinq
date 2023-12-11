using ExRam.Gremlinq.Core;

using System.Buffers;

namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct GraphSon2MessageBuffer : IMessageBuffer
    {
        private readonly IMemoryOwner<byte>? _owner;

        public GraphSon2MessageBuffer(IMemoryOwner<byte> owner)
        {
            _owner = owner;
        }

        public void Dispose() => _owner?.Dispose();

        public Memory<byte> Memory => _owner?.Memory ?? throw ExceptionHelper.UninitializedStruct();
    }
}
