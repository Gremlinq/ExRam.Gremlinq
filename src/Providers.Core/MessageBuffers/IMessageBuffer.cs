namespace ExRam.Gremlinq.Providers.Core
{
    public interface IMessageBuffer
    {
        ReadOnlyMemory<byte> Memory { get; }
    }
}
