#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core.Tests.Extensions;

using FluentAssertions;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

using NSubstitute;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class PoolTests
    {
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
                .GetAsyncEnumerator(TestContext.Current.CancellationToken)
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
                .GetAsyncEnumerator(TestContext.Current.CancellationToken)
                .MoveNextAsync();

            await poolClient
                .SubmitAsync<int>(RequestMessage.Build("op").Create())
                .AsAsyncEnumerable()
                .GetAsyncEnumerator(TestContext.Current.CancellationToken)
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
                .GetAsyncEnumerator(TestContext.Current.CancellationToken)
                .MoveNextAsync()
                .AsTask();

            poolClient
                .SubmitAsync<int>(RequestMessage.Build("op").Create())
                .AsAsyncEnumerable()
                .GetAsyncEnumerator(TestContext.Current.CancellationToken)
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
                    .GetAsyncEnumerator(TestContext.Current.CancellationToken)
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
                .GetAsyncEnumerator(TestContext.Current.CancellationToken)
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
                    .GetAsyncEnumerator(TestContext.Current.CancellationToken)
                    .Awaiting(__ => __
                        .MoveNextAsync())
                    .Should()
                    .ThrowAsync<DivideByZeroException>();
            }

            baseFactory
                .Received(3)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }

        [Fact]
        public async Task Retry_once_upon_ObjectDisposedException()
        {
            var faultyClient = Substitute
                .For<IGremlinqClient>();

            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            faultyClient
                .SubmitAsync<int>(Arg.Any<RequestMessage>())
                .Returns(
                    AsyncEnumerableEx.Throw<ResponseMessage<int>>(new ObjectDisposedException(null)),
                    AsyncEnumerableEx.Return(new ResponseMessage<int>(default, new ResponseStatus(ResponseStatusCode.Success), new ResponseResult<int>(12))));

            baseFactory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(faultyClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            var result = await System.Linq.AsyncEnumerable
                .ToArrayAsync(poolClient
                    .SubmitAsync<int>(RequestMessage.Build("op").Create()), TestContext.Current.CancellationToken);

            baseFactory
                .Received(2)
                .Create(Arg.Any<IGremlinQueryEnvironment>());

            await Verify(result);
        }

        [Fact]
        public async Task Retry_only_once_upon_ObjectDisposedException()
        {
            var faultyClient = Substitute
                .For<IGremlinqClient>();

            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            faultyClient
                .SubmitAsync<int>(Arg.Any<RequestMessage>())
                .Returns(
                    AsyncEnumerableEx.Throw<ResponseMessage<int>>(new ObjectDisposedException(null)),
                    AsyncEnumerableEx.Throw<ResponseMessage<int>>(new ObjectDisposedException(null)),
                    AsyncEnumerableEx.Return(new ResponseMessage<int>(default, new ResponseStatus(ResponseStatusCode.Success), new ResponseResult<int>(12))));

            baseFactory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(faultyClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            poolClient
                .SubmitAsync<int>(RequestMessage.Build("op").Create())
                .AsAsyncEnumerable()
                .GetAsyncEnumerator(TestContext.Current.CancellationToken)
                .Awaiting(__ => __
                    .MoveNextAsync())
                .Should()
                .ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public void Pool_disposes_its_subClients()
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
                .GetAsyncEnumerator(TestContext.Current.CancellationToken)
                .MoveNextAsync();

            poolClient.Dispose();

            neverClient
                .Received(1)
                .Dispose();
        }

        [Fact]
        public void Requests_of_a_session_stay_on_the_client_they_started_on()
        {
            var clients = new List<IGremlinqClient>();

            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            baseFactory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(_ =>
                {
                    var client = CreateClient(AsyncEnumerableEx.Never<ResponseMessage<int>>());

                    clients.Add(client);

                    return client;
                });

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            poolClient.Start(SessionRequest("session-1"));

            for (var i = 0; i < 7; i++)
            {
                poolClient.Start(SessionlessRequest());
            }

            for (var i = 0; i < 3; i++)
            {
                poolClient.Start(SessionRequest("session-1"));
                poolClient.Start(SessionlessRequest());
            }

            var sessionClients = clients
                .Where(client => client.ReceivedRequests().Any(request => request.SessionId() is "session-1"))
                .ToArray();

            sessionClients
                .Should()
                .ContainSingle();

            sessionClients[0]
                .ReceivedRequests()
                .Count(request => request.SessionId() is "session-1")
                .Should()
                .Be(4);

            clients
                .Count(client => client.ReceivedRequests().Any(request => request.SessionId() is null))
                .Should()
                .BeGreaterThan(1);
        }

        [Fact]
        public void Two_sessions_may_share_one_client()
        {
            var client = CreateClient(AsyncEnumerableEx.Never<ResponseMessage<int>>());
            var baseFactory = CreateFactory(client);

            var poolClient = baseFactory
                .Pool()
                .WithPoolSize(1)
                .Create(GremlinQueryEnvironment.Invalid);

            poolClient.Start(SessionRequest("session-1"));
            poolClient.Start(SessionRequest("session-2"));

            baseFactory
                .Received(1)
                .Create(Arg.Any<IGremlinQueryEnvironment>());

            client
                .ReceivedRequests()
                .Select(request => request.SessionId())
                .Should()
                .Equal("session-1", "session-2");
        }

        [Fact]
        public async Task Request_of_a_session_whose_client_was_dropped_throws()
        {
            var client = CreateClient(Success(), AsyncEnumerableEx.Throw<ResponseMessage<int>>(new DivideByZeroException()));
            var baseFactory = CreateFactory(client);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient.Submit(SessionRequest("session-1"));

            await poolClient
                .Awaiting(_ => poolClient.Submit(SessionlessRequest()))
                .Should()
                .ThrowAsync<DivideByZeroException>();

            client
                .Received(1)
                .Dispose();

            await poolClient
                .Awaiting(_ => poolClient.Submit(SessionRequest("session-1")))
                .Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("*session-1*");

            baseFactory
                .Received(1)
                .Create(Arg.Any<IGremlinQueryEnvironment>());

            client
                .ReceivedRequests()
                .Should()
                .HaveCount(2);
        }

        [Fact]
        public async Task Requests_of_a_session_whose_client_was_replaced_throw()
        {
            var firstClient = CreateClient(Success(), AsyncEnumerableEx.Throw<ResponseMessage<int>>(new DivideByZeroException()));
            var secondClient = CreateClient(Success());
            var baseFactory = CreateFactory(firstClient, secondClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient.Submit(SessionRequest("session-1"));

            await poolClient
                .Awaiting(_ => poolClient.Submit(SessionlessRequest()))
                .Should()
                .ThrowAsync<DivideByZeroException>();

            await poolClient.Submit(SessionlessRequest());

            for (var i = 0; i < 2; i++)
            {
                await poolClient
                    .Awaiting(_ => poolClient.Submit(SessionRequest("session-1")))
                    .Should()
                    .ThrowAsync<InvalidOperationException>()
                    .WithMessage("*session-1*");
            }

            baseFactory
                .Received(2)
                .Create(Arg.Any<IGremlinQueryEnvironment>());

            firstClient
                .ReceivedRequests()
                .Should()
                .HaveCount(2);

            secondClient
                .ReceivedRequests()
                .Should()
                .ContainSingle(request => request.SessionId() == null);
        }

        [Fact]
        public async Task First_request_of_a_session_is_not_retried_upon_ObjectDisposedException()
        {
            var baseFactory = CreateFactory(CreateClient(
                AsyncEnumerableEx.Throw<ResponseMessage<int>>(new ObjectDisposedException(null)),
                Success()));

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient
                .Awaiting(_ => poolClient.Submit(SessionRequest("session-1")))
                .Should()
                .ThrowAsync<ObjectDisposedException>();

            baseFactory
                .Received(1)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }

        [Fact]
        public async Task Later_request_of_a_session_is_not_retried_upon_ObjectDisposedException()
        {
            var baseFactory = CreateFactory(CreateClient(
                Success(),
                AsyncEnumerableEx.Throw<ResponseMessage<int>>(new ObjectDisposedException(null)),
                Success()));

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient.Submit(SessionRequest("session-1"));

            await poolClient
                .Awaiting(_ => poolClient.Submit(SessionRequest("session-1")))
                .Should()
                .ThrowAsync<ObjectDisposedException>();

            baseFactory
                .Received(1)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Request_of_the_session_processor_without_session_id_is_retried_upon_ObjectDisposedException(bool withNullSessionId)
        {
            var baseFactory = CreateFactory(CreateClient(
                AsyncEnumerableEx.Throw<ResponseMessage<int>>(new ObjectDisposedException(null)),
                Success()));

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            var builder = RequestMessage
                .Build(Tokens.OpsBytecode)
                .Processor(Tokens.ProcessorSession);

            if (withNullSessionId)
                builder = builder.AddArgument(Tokens.ArgsSession, null!);

            await poolClient.Submit(builder.Create());

            baseFactory
                .Received(2)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }

        [Fact]
        public async Task Close_releases_the_session()
        {
            var firstClient = CreateClient(Success(), Success(), AsyncEnumerableEx.Throw<ResponseMessage<int>>(new DivideByZeroException()));
            var secondClient = CreateClient(Success());
            var baseFactory = CreateFactory(firstClient, secondClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient.Submit(SessionRequest("session-1"));
            await poolClient.Submit(CloseRequest("session-1"));

            await poolClient
                .Awaiting(_ => poolClient.Submit(SessionlessRequest()))
                .Should()
                .ThrowAsync<DivideByZeroException>();

            await poolClient.Submit(SessionRequest("session-1"));

            firstClient
                .ReceivedRequests()
                .Select(request => request.Operation)
                .Should()
                .Equal(Tokens.OpsBytecode, Tokens.OpsClose, "op");

            secondClient
                .ReceivedRequests()
                .Select(request => request.SessionId())
                .Should()
                .Equal("session-1");
        }

        [Fact]
        public async Task Close_answered_with_an_error_releases_the_session()
        {
            var firstClient = CreateClient(
                Success(),
                AsyncEnumerableEx.Return(new ResponseMessage<int>(default, new ResponseStatus(ResponseStatusCode.ServerError), new ResponseResult<int>(0))),
                AsyncEnumerableEx.Throw<ResponseMessage<int>>(new DivideByZeroException()));

            var secondClient = CreateClient(Success());
            var baseFactory = CreateFactory(firstClient, secondClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient.Submit(SessionRequest("session-1"));
            await poolClient.Submit(CloseRequest("session-1"));

            await poolClient
                .Awaiting(_ => poolClient.Submit(SessionlessRequest()))
                .Should()
                .ThrowAsync<DivideByZeroException>();

            await poolClient.Submit(SessionRequest("session-1"));

            secondClient
                .ReceivedRequests()
                .Select(request => request.SessionId())
                .Should()
                .Equal("session-1");
        }

        [Fact]
        public async Task Close_that_throws_releases_the_session()
        {
            var firstClient = CreateClient(Success(), AsyncEnumerableEx.Throw<ResponseMessage<int>>(new DivideByZeroException()));
            var secondClient = CreateClient(Success());
            var baseFactory = CreateFactory(firstClient, secondClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient.Submit(SessionRequest("session-1"));

            await poolClient
                .Awaiting(_ => poolClient.Submit(CloseRequest("session-1")))
                .Should()
                .ThrowAsync<DivideByZeroException>();

            await poolClient.Submit(SessionRequest("session-1"));

            secondClient
                .ReceivedRequests()
                .Select(request => request.SessionId())
                .Should()
                .Equal("session-1");
        }

        [Fact]
        public async Task Close_of_a_session_whose_client_was_dropped_throws_and_releases_the_session()
        {
            var firstClient = CreateClient(Success(), AsyncEnumerableEx.Throw<ResponseMessage<int>>(new DivideByZeroException()));
            var secondClient = CreateClient(Success());
            var baseFactory = CreateFactory(firstClient, secondClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient.Submit(SessionRequest("session-1"));

            await poolClient
                .Awaiting(_ => poolClient.Submit(SessionlessRequest()))
                .Should()
                .ThrowAsync<DivideByZeroException>();

            await poolClient
                .Awaiting(_ => poolClient.Submit(CloseRequest("session-1")))
                .Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("*session-1*");

            baseFactory
                .Received(1)
                .Create(Arg.Any<IGremlinQueryEnvironment>());

            await poolClient.Submit(SessionRequest("session-1"));

            firstClient
                .ReceivedRequests()
                .Should()
                .HaveCount(2);

            secondClient
                .ReceivedRequests()
                .Select(request => request.SessionId())
                .Should()
                .Equal("session-1");
        }

        [Fact]
        public async Task Close_of_a_session_without_binding_binds_nothing()
        {
            var firstClient = CreateClient(Success(), AsyncEnumerableEx.Throw<ResponseMessage<int>>(new DivideByZeroException()));
            var secondClient = CreateClient(Success());
            var baseFactory = CreateFactory(firstClient, secondClient);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient.Submit(CloseRequest("session-1"));

            await poolClient
                .Awaiting(_ => poolClient.Submit(SessionlessRequest()))
                .Should()
                .ThrowAsync<DivideByZeroException>();

            await poolClient.Submit(SessionRequest("session-1"));

            firstClient
                .ReceivedRequests()
                .Select(request => request.Operation)
                .Should()
                .Equal(Tokens.OpsClose, "op");

            secondClient
                .ReceivedRequests()
                .Select(request => request.SessionId())
                .Should()
                .Equal("session-1");
        }

        [Fact]
        public async Task Close_of_a_session_without_binding_is_not_retried_upon_ObjectDisposedException()
        {
            var baseFactory = CreateFactory(CreateClient(
                AsyncEnumerableEx.Throw<ResponseMessage<int>>(new ObjectDisposedException(null)),
                Success()));

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            await poolClient
                .Awaiting(_ => poolClient.Submit(CloseRequest("session-1")))
                .Should()
                .ThrowAsync<ObjectDisposedException>();

            baseFactory
                .Received(1)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }

        [Fact]
        public async Task Pool_disposal_releases_the_sessions()
        {
            var client = CreateClient(AsyncEnumerableEx.Never<ResponseMessage<int>>());
            var baseFactory = CreateFactory(client);

            var poolClient = baseFactory
                .Pool()
                .Create(GremlinQueryEnvironment.Invalid);

            poolClient.Start(SessionRequest("session-1"));

            poolClient.Dispose();

            client
                .Received(1)
                .Dispose();

            await poolClient
                .Awaiting(_ => poolClient.Submit(SessionRequest("session-1")))
                .Should()
                .ThrowAsync<ObjectDisposedException>();

            baseFactory
                .Received(1)
                .Create(Arg.Any<IGremlinQueryEnvironment>());
        }

        [Fact]
        public void Requests_racing_for_the_first_connection_of_a_slot_share_one()
        {
            var poolClient = default(IGremlinqClient);
            var losingClient = CreateClient(AsyncEnumerableEx.Never<ResponseMessage<int>>());
            var winningClient = CreateClient(AsyncEnumerableEx.Never<ResponseMessage<int>>());

            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            baseFactory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(
                    _ =>
                    {
                        poolClient!.Start(SessionlessRequest());

                        return losingClient;
                    },
                    _ => winningClient);

            poolClient = baseFactory
                .Pool()
                .WithPoolSize(1)
                .Create(GremlinQueryEnvironment.Invalid);

            poolClient.Start(SessionlessRequest());

            baseFactory
                .Received(2)
                .Create(Arg.Any<IGremlinQueryEnvironment>());

            losingClient
                .Received(1)
                .Dispose();

            losingClient
                .ReceivedRequests()
                .Should()
                .BeEmpty();

            winningClient
                .ReceivedRequests()
                .Should()
                .HaveCount(2);
        }

        private static IGremlinqClient CreateClient(IAsyncEnumerable<ResponseMessage<int>> responses, params IAsyncEnumerable<ResponseMessage<int>>[] laterResponses)
        {
            var client = Substitute
                .For<IGremlinqClient>();

            client
                .SubmitAsync<int>(Arg.Any<RequestMessage>())
                .Returns(responses, laterResponses);

            return client;
        }

        private static IGremlinqClientFactory CreateFactory(IGremlinqClient client, params IGremlinqClient[] laterClients)
        {
            var baseFactory = Substitute
                .For<IGremlinqClientFactory>();

            baseFactory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(client, laterClients);

            return baseFactory;
        }

        private static IAsyncEnumerable<ResponseMessage<int>> Success() => AsyncEnumerableEx.Return(new ResponseMessage<int>(default, new ResponseStatus(ResponseStatusCode.Success), new ResponseResult<int>(12)));

        private static RequestMessage SessionlessRequest() => RequestMessage
            .Build("op")
            .Create();

        private static RequestMessage SessionRequest(string sessionId) => RequestMessage
            .Build(Tokens.OpsBytecode)
            .Processor(Tokens.ProcessorSession)
            .AddArgument(Tokens.ArgsSession, sessionId)
            .Create();

        private static RequestMessage CloseRequest(string sessionId) => RequestMessage
            .Build(Tokens.OpsClose)
            .Processor(Tokens.ProcessorSession)
            .AddArgument(Tokens.ArgsSession, sessionId)
            .Create();
    }
}
