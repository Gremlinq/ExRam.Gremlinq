using ExRam.Gremlinq.Core;

using System.Buffers;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// A binary message encoded in GraphSON v2 format for WebSocket communication.
    /// </summary>
    public readonly struct GraphSon2BinaryMessage : IMemoryOwner<byte>
    {
        private readonly IMemoryOwner<byte>? _owner;

        /// <summary>
        /// Initializes a new <see cref="GraphSon2BinaryMessage"/> wrapping the specified memory owner.
        /// </summary>
        /// <param name="owner">The memory owner that holds the binary data.</param>
        public GraphSon2BinaryMessage(IMemoryOwner<byte> owner)
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
