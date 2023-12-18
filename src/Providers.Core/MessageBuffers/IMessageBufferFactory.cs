using System.Buffers;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IMessageBufferFactory<TBuffer>
        where TBuffer : IMemoryOwner<byte>
    {
        TBuffer Create(IMemoryOwner<byte> message);
    }
}
