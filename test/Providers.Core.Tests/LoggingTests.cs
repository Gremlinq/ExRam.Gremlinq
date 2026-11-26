using System.Collections.Immutable;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;

using FluentAssertions;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

using Microsoft.Extensions.Logging;

using NSubstitute;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class LoggingTests
    {
        private static readonly Guid RequestId = Guid.Parse("8e6f1a4c-2b4e-4d0a-9c39-0b6f5d0f3e11");

        [Fact]
        public void Eval_request_is_logged_with_its_script()
        {
            var logger = new CollectingLogger();

            Submit(logger, QueryLogVerbosity.QueryOnly);

            logger.Messages
                .Should()
                .Equal($"Executing Gremlin query {RequestId} with groovy script g.V(_a).");
        }

        [Fact]
        public void Eval_request_is_logged_with_its_script_and_bindings()
        {
            var logger = new CollectingLogger();

            Submit(logger, QueryLogVerbosity.IncludeBindings);

            logger.Messages
                .Should()
                .Equal($"Executing Gremlin query {RequestId} with groovy script g.V(_a) and parameter bindings [_a, 1].");
        }

        [Fact]
        public void Log_dispose_propagates()
        {
            var inner = Substitute.For<IGremlinqClient>();

            inner
                .Log(GremlinQueryEnvironment.Invalid)
                .Dispose();

            inner.Received(1).Dispose();
        }

        [Fact]
        public async Task Failed_query_is_logged_as_an_error()
        {
            var logger = new CollectingLogger();
            var context = CreateContext(logger);

            var execution = () => Execute(context, AsyncEnumerableEx.Throw<ResponseMessage<List<object>>>(new DivideByZeroException()), TestContext.Current.CancellationToken);

            await execution
                .Should()
                .ThrowAsync<GremlinQueryExecutionException>();

            logger.Entries
                .Should()
                .Equal((LogLevel.Error, $"Execution of Gremlin query {context.ExecutionId} failed."));
        }

        [Fact]
        public async Task Query_cancelled_while_its_token_is_not_is_logged_as_an_error()
        {
            var logger = new CollectingLogger();
            var context = CreateContext(logger);

            var execution = () => Execute(context, AsyncEnumerableEx.Throw<ResponseMessage<List<object>>>(new TaskCanceledException()), TestContext.Current.CancellationToken);

            await execution
                .Should()
                .ThrowAsync<OperationCanceledException>();

            logger.Entries
                .Should()
                .Equal((LogLevel.Error, $"Execution of Gremlin query {context.ExecutionId} failed."));
        }

        [Fact]
        public async Task Query_cancelled_through_its_token_is_logged_at_the_query_log_level()
        {
            var logger = new CollectingLogger();
            var context = CreateContext(logger);

            await Cancel(context);

            logger.Entries
                .Should()
                .Equal((LogLevel.Debug, $"Execution of Gremlin query {context.ExecutionId} was cancelled."));
        }

        [Fact]
        public async Task Query_cancelled_through_its_token_is_logged_at_a_configured_query_log_level()
        {
            var logger = new CollectingLogger();
            var context = CreateContext(logger, options => options
                .SetValue(GremlinqOption.QueryLogLogLevel, LogLevel.Information));

            await Cancel(context);

            logger.Entries
                .Should()
                .Equal((LogLevel.Information, $"Execution of Gremlin query {context.ExecutionId} was cancelled."));
        }

        private static async Task Cancel(GremlinQueryExecutionContext context)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);

            var execution = Execute(context, AsyncEnumerableEx.Never<ResponseMessage<List<object>>>(), cts.Token);

            await cts.CancelAsync();

            await FluentActions
                .Awaiting(() => execution)
                .Should()
                .ThrowAsync<OperationCanceledException>();
        }

        private static GremlinQueryExecutionContext CreateContext(ILogger logger, Func<IGremlinqOptions, IGremlinqOptions>? optionsTransformation = null) => GremlinQueryExecutionContext.Create(GremlinQuerySource.g
            .ConfigureEnvironment(environment => environment
                .UseLogger(logger)
                .ConfigureOptions(optionsTransformation ?? (options => options)))
            .V());

        private static async Task Execute(GremlinQueryExecutionContext context, IAsyncEnumerable<ResponseMessage<List<object>>> responses, CancellationToken ct)
        {
            var client = Substitute.For<IGremlinqClient>();
            var factory = Substitute.For<IGremlinqClientFactory>();

            factory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(client);

            client
                .SubmitAsync<List<object>>(Arg.Any<RequestMessage>())
                .Returns(responses);

            await foreach (var _ in factory.ToExecutor().Execute<object>(context).WithCancellation(ct))
            {
            }
        }

        private static void Submit(ILogger logger, QueryLogVerbosity verbosity)
        {
            var inner = Substitute.For<IGremlinqClient>();

            inner
                .SubmitAsync<int>(Arg.Any<RequestMessage>())
                .Returns(AsyncEnumerableEx.Never<ResponseMessage<int>>());

            inner
                .Log(GremlinQueryEnvironment.Invalid
                    .UseLogger(logger)
                    .ConfigureOptions(options => options
                        .SetValue(GremlinqOption.QueryLogLogLevel, LogLevel.Information)
                        .SetValue(GremlinqOption.QueryLogVerbosity, verbosity)))
                .SubmitAsync<int>(RequestMessage
                    .Build(Tokens.OpsEval)
                    .OverrideRequestId(RequestId)
                    .AddArgument(Tokens.ArgsGremlin, "g.V(_a)")
                    .AddArgument(Tokens.ArgsBindings, ImmutableDictionary<string, object?>.Empty.Add("_a", 1))
                    .Create())
                .GetAsyncEnumerator(TestContext.Current.CancellationToken)
                .MoveNextAsync();
        }

        private sealed class CollectingLogger : ILogger
        {
            public List<(LogLevel Level, string Message)> Entries { get; } = [];

            public IEnumerable<string> Messages => Entries.Select(entry => entry.Message);

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) => Entries.Add((logLevel, formatter(state, exception)));
        }
    }
}
