using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// A client that can submit Gremlin request messages to a server and stream back response messages.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A request whose <see cref="RequestMessage.Processor"/> is <see cref="Tokens.ProcessorSession"/> and whose
    /// <see cref="RequestMessage.Arguments"/> hold a <see cref="Tokens.ArgsSession"/> is a session request. A server may bind
    /// a session to the connection it was created on, and ends it when that connection closes. A client that spreads
    /// requests over several connections therefore sends the first session request of an id on one connection and every
    /// later session request of that id on the same connection instance. Sessionless requests and other sessions may use
    /// that connection as well.
    /// </para>
    /// <para>
    /// A session never moves to another connection. Once the connection of a session was closed or replaced, every later
    /// request of that session fails before anything is sent, with an exception that names the session, until a close
    /// request releases the session. No session request is retried. A request whose <see cref="RequestMessage.Operation"/>
    /// is <see cref="Tokens.OpsClose"/> is sent on the connection of its session and releases the session when it is sent,
    /// whatever the server answers.
    /// </para>
    /// </remarks>
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
