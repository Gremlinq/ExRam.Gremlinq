using System.Collections.Immutable;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;

using FluentAssertions;

using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class ExceptionExtensionsEdgeCaseTests
    {
        private static GremlinQueryExecutionException CreateException(Exception? inner = null) =>
            new(GremlinQueryExecutionContext.Create(GremlinQuerySource.g.Inject(42)), inner ?? new Exception());

        [Fact]
        public void Non_ResponseException_returns_null()
        {
            var ex = CreateException(new InvalidOperationException("not a response exception"));

            ex.TryGetNeptuneGremlinQueryExecutionException()
                .Should().BeNull();
        }

        [Fact]
        public void ResponseException_with_invalid_json_returns_null()
        {
            var ex = CreateException(new ResponseException(
                ResponseStatusCode.ServerError,
                ImmutableDictionary<string, object>.Empty,
                "ServerError: this is not valid json"));

            ex.TryGetNeptuneGremlinQueryExecutionException()
                .Should().BeNull();
        }

        [Fact]
        public void ResponseException_without_code_returns_null()
        {
            var ex = CreateException(new ResponseException(
                ResponseStatusCode.ServerError,
                ImmutableDictionary<string, object>.Empty,
                """ServerError: {"detailedMessage":"some detail","requestId":"abc"}"""));

            ex.TryGetNeptuneGremlinQueryExecutionException()
                .Should().BeNull();
        }

        [Fact]
        public void ResponseException_with_empty_code_returns_null()
        {
            var ex = CreateException(new ResponseException(
                ResponseStatusCode.ServerError,
                ImmutableDictionary<string, object>.Empty,
                """ServerError: {"code":"","detailedMessage":"detail","requestId":"abc"}"""));

            ex.TryGetNeptuneGremlinQueryExecutionException()
                .Should().BeNull();
        }

        [Fact]
        public void ResponseException_message_equals_status_code_only_returns_null()
        {
            var ex = CreateException(new ResponseException(
                ResponseStatusCode.ServerError,
                ImmutableDictionary<string, object>.Empty,
                "ServerError"));

            ex.TryGetNeptuneGremlinQueryExecutionException()
                .Should().BeNull();
        }

        [Fact]
        public void UseDFE_null_source_throws()
        {
            FluentActions.Invoking(() => GremlinQuerySourceExtensions.UseDFE(null!))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UseDFE_enabled_returns_source()
        {
            GremlinQuerySource.g.UseDFE()
                .Should().NotBeNull();
        }

        [Fact]
        public void UseDFE_disabled_returns_source()
        {
            GremlinQuerySource.g.UseDFE(false)
                .Should().NotBeNull();
        }
    }
}
