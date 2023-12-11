namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct GraphSon3MessageBuffer : IMessageBuffer
    {
        public GraphSon3MessageBuffer(ReadOnlyMemory<byte> memory) : this()
        {
            Memory = memory;
        }

        public ReadOnlyMemory<byte> Memory { get; }
    }
}
