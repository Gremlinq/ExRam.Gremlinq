namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct GraphSon2MessageBuffer : IMessageBuffer
    {
        public GraphSon2MessageBuffer(ReadOnlyMemory<byte> memory) : this()
        {
            Memory = memory;
        }

        public ReadOnlyMemory<byte> Memory { get; }
    }
}
