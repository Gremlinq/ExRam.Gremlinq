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

        public string GetMimeType() => "application/vnd.gremlin-v3.0+json";

        public Memory<byte> Memory => _owner?.Memory ?? throw ExceptionHelper.UninitializedStruct();
    }
}
