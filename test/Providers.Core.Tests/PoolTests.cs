#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

using ExRam.Gremlinq.Core;

using FluentAssertions;

using Gremlin.Net.Driver.Messages;

using NSubstitute;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class PoolTests : VerifyBase
    {
        public PoolTests() : base()
        {

        }

        [Fact]
        public async Task Pool_creates_subClient()
        {
            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            baseFactory
                .DidNotReceive()
                .Create(Arg.Any<IGremlinQueryEnvironment>());

            await poolClient
                .SubmitAsync<int>(RequestMessage.Build("op").Create())
                .AsAsyncEnumerable()
                .GetAsyncEnumerator()
                .MoveNextAsync();

            baseFactory
                .Received(1)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }

        [Fact]
        public async Task Pool_creates_only_one_subClient_when_not_overlapping()
        {
            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient
                .SubmitAsync<int>(RequestMessage.Build("op").Create())
                .AsAsyncEnumerable()
                .GetAsyncEnumerator()
                .MoveNextAsync();

            await poolClient
                .SubmitAsync<int>(RequestMessage.Build("op").Create())
                .AsAsyncEnumerable()
                .GetAsyncEnumerator()
                .MoveNextAsync();

            baseFactory
                .Received(1)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }

        [Fact]
        public async Task Pool_creates_second_client_when_overlapping()
        {
            var neverClient = Substitute
                .For<IGremlinqClient>();

            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            neverClient
                .SubmitAsync<int>(Arg.Any<RequestMessage>())
                .Returns(AsyncEnumerableEx.Never<ResponseMessage<int>>());

            baseFactory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(neverClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            poolClient
                .SubmitAsync<int>(RequestMessage.Build("op").Create())
                .AsAsyncEnumerable()
                .GetAsyncEnumerator()
                .MoveNextAsync()
                .AsTask();

            poolClient
                .SubmitAsync<int>(RequestMessage.Build("op").Create())
                .AsAsyncEnumerable()
                .GetAsyncEnumerator()
                .MoveNextAsync()
                .AsTask();

            baseFactory
                .Received(2)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }

        [Fact]
        public async Task Pool_creates_not_more_clients_than_there_are_slots()
        {
            var neverClient = Substitute
                .For<IGremlinqClient>();

            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            neverClient
                .SubmitAsync<int>(Arg.Any<RequestMessage>())
                .Returns(AsyncEnumerableEx.Never<ResponseMessage<int>>());

            baseFactory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(neverClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            for (var i = 0; i < 1000; i++)
            {
                poolClient
                    .SubmitAsync<int>(RequestMessage.Build("op").Create())
                    .AsAsyncEnumerable()
                    .GetAsyncEnumerator()
                    .MoveNextAsync()
                    .AsTask();
            }

            baseFactory
                .Received(8)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }

        [Fact]
        public async Task Exception_is_forwarded()
        {
            var faulyClient = Substitute
                .For<IGremlinqClient>();

            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            faulyClient
                .SubmitAsync<int>(Arg.Any<RequestMessage>())
                .Returns(AsyncEnumerableEx.Throw<ResponseMessage<int>>(new DivideByZeroException()));

            baseFactory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(faulyClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient
                .SubmitAsync<int>(RequestMessage.Build("op").Create())
                .AsAsyncEnumerable()
                .GetAsyncEnumerator()
                .Awaiting(__ => __
                    .MoveNextAsync())
                .Should()
                .ThrowAsync<DivideByZeroException>();
        }

        [Fact]
        public async Task Client_is_recreated()
        {
            var faulyClient = Substitute
                .For<IGremlinqClient>();

            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            faulyClient
                .SubmitAsync<int>(Arg.Any<RequestMessage>())
                .Returns(AsyncEnumerableEx.Throw<ResponseMessage<int>>(new DivideByZeroException()));

            baseFactory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(faulyClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            for (var i = 0; i < 3; i++)
            {
                poolClient
                    .SubmitAsync<int>(RequestMessage.Build("op").Create())
                    .AsAsyncEnumerable()
                    .GetAsyncEnumerator()
                    .Awaiting(__ => __
                        .MoveNextAsync())
                    .Should()
                    .ThrowAsync<DivideByZeroException>();
            }

            baseFactory
                .Received(3)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }
    }
}
