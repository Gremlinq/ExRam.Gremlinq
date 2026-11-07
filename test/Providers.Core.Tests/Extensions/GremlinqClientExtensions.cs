using Gremlin.Net.Driver.Messages;

using NSubstitute;

namespace ExRam.Gremlinq.Providers.Core.Tests.Extensions
{
    internal static class GremlinqClientExtensions
    {
        public static void Start(this IGremlinqClient client, RequestMessage request) => _ = client
            .SubmitAsync<int>(request)
            .GetAsyncEnumerator(TestContext.Current.CancellationToken)
            .MoveNextAsync()
            .AsTask();

        public static async Task Submit(this IGremlinqClient client, RequestMessage request) => await System.Linq.AsyncEnumerable
            .ToArrayAsync(client.SubmitAsync<int>(request), TestContext.Current.CancellationToken);

        public static RequestMessage[] ReceivedRequests(this IGremlinqClient client) => client
            .ReceivedCalls()
            .SelectMany(call => call.GetArguments().OfType<RequestMessage>())
            .ToArray();
    }
}
