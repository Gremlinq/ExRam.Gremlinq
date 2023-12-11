using Gremlin.Net.Driver.Messages;

using System.Buffers;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IMessageBufferFactory<TBuffer>
        where TBuffer : IMessageBuffer
    {
        TBuffer Create(RequestMessage message);

        TBuffer Create(IMemoryOwner<byte> message);

        string MimeType { get; }
    }
}
