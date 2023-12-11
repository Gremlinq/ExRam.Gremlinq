using System.Buffers;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IMessageBuffer : IMemoryOwner<byte>
    {
    }
}
