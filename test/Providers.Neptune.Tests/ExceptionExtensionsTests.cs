using System.Collections.Immutable;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class ExceptionExtensionsTests
    {
        [Fact]
        public Task ServerError_InternalFailureException() => Verify(new GremlinQueryExecutionException(
            GremlinQueryExecutionContext.Create(GremlinQuerySource.g.Inject(42)),
            new ResponseException(
                ResponseStatusCode.ServerError,
                ImmutableDictionary<string, object>.Empty,
                """
                ServerError: {"detailedMessage":"The variable a for math() step must resolve to a Number - it is instead of type null with value null","code":"InternalFailureException","requestId":"1bfc68f6-b58e-4b55-b411-cd5552d27f0b","message":"The variable a for math() step must resolve to a Number - it is instead of type null with value null"}
                """))
            .TryGetNeptuneGremlinQueryExecutionException())
            .ScrubGuidsWithConstant();

        [Fact]
        public Task ServerError_InternalFailureException_no_DetailedMessage() => Verify(new GremlinQueryExecutionException(
           GremlinQueryExecutionContext.Create(GremlinQuerySource.g.Inject(42)),
           new ResponseException(
               ResponseStatusCode.ServerError,
               ImmutableDictionary<string, object>.Empty,
               """
                ServerError: {"code":"InternalFailureException","requestId":"1bfc68f6-b58e-4b55-b411-cd5552d27f0b","message":"The variable a for math() step must resolve to a Number - it is instead of type null with value null"}
                """))
           .TryGetNeptuneGremlinQueryExecutionException())
           .ScrubGuidsWithConstant();
    }
}
