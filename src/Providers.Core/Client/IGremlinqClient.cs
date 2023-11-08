using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IGremlinqClient : IDisposable
    {
        IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message);
    }
}
