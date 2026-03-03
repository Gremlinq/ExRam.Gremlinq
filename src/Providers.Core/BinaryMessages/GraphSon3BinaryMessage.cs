using System.Buffers;

using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// A binary message encoded in GraphSON v3 format for WebSocket communication.
    /// </summary>
    public readonly struct GraphSon3BinaryMessage : IMemoryOwner<byte>
    {
        private readonly IMemoryOwner<byte>? _owner;

        /// <summary>
        /// Initializes a new <see cref="GraphSon3BinaryMessage"/> wrapping the specified memory owner.
        /// </summary>
        /// <param name="owner">The memory owner that holds the binary data.</param>
        public GraphSon3BinaryMessage(IMemoryOwner<byte> owner)
        {
            ArgumentNullException.ThrowIfNull(owner);

            _owner = owner;
        }

        /// <inheritdoc />
        public void Dispose() => _owner?.Dispose();

        /// <inheritdoc />
        public Memory<byte> Memory => _owner?.Memory ?? throw ExceptionHelper.UninitializedStruct();
    }
}
