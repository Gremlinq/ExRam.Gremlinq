using ExRam.Gremlinq.Core;

using FluentAssertions;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class WebSocketClientTests
    {
        [Fact]
        public async Task Request_the_serializer_cannot_write_fails_before_connecting()
        {
            using var client = WebSocketGremlinqClientFactory.LocalHost
                .Create(GremlinQueryEnvironment.Invalid);

            await client
                .Awaiting(client => client
                    .SubmitAsync<object>(RequestMessage.Build("op").Create())
                    .GetAsyncEnumerator(TestContext.Current.CancellationToken)
                    .MoveNextAsync()
                    .AsTask())
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }
    }
}
