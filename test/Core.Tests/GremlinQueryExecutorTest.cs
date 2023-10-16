using ExRam.Gremlinq.Core.Execution;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryExecutorTest
    {
        private readonly IVertexGremlinQuery<object> _query;

        public GremlinQueryExecutorTest()
        {
            _query = GremlinQuerySource.g.ConfigureEnvironment(_ => _).V();
        }

        [Fact]
        public void Invalid()
        {
            GremlinQueryExecutor.Invalid
                .Execute<object>(GremlinQueryExecutionContext.Create(_query))
                .Awaiting(ex => ex
                    .ToArrayAsync())
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }
    }
}
