using System.Collections.Immutable;

using ExRam.Gremlinq.Core;

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
            public List<string> Messages { get; } = [];

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) => Messages.Add(formatter(state, exception));
        }
    }
}
