using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// A client that can submit Gremlin request messages to a server and stream back response messages.
    /// </summary>
    public interface IGremlinqClient : IDisposable
    {
        /// <summary>
        /// Submits a request message and returns an async stream of response messages.
        /// </summary>
        /// <typeparam name="T">The type of the response message payload.</typeparam>
        /// <param name="message">The request message to submit.</param>
        IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message);
    }
}
