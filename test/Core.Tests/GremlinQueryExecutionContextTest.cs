using ExRam.Gremlinq.Core.Execution;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryExecutionContextTest
    {
        private readonly IVertexGremlinQuery<object> _query;

        public GremlinQueryExecutionContextTest()
        {
            _query = GremlinQuerySource.g.V();
        }

        [Fact]
        public void Create_sets_query()
        {
            var context = GremlinQueryExecutionContext.Create(_query);

            context.Query
                .Should()
                .BeSameAs(_query);
        }

        [Fact]
        public void Create_sets_execution_id()
        {
            var context = GremlinQueryExecutionContext.Create(_query);

            context.ExecutionId
                .Should()
                .NotBeEmpty();
        }

        [Fact]
        public void WithNewExecutionId_changes_execution_id()
        {
            var context = GremlinQueryExecutionContext.Create(_query);
            var newContext = context.WithNewExecutionId();

            newContext.ExecutionId
                .Should()
                .NotBe(context.ExecutionId);

            newContext.Query
                .Should()
                .BeSameAs(context.Query);
        }

        [Fact]
        public void TransformQuery_applies_transformation()
        {
            var context = GremlinQueryExecutionContext.Create(_query);
            var otherQuery = GremlinQuerySource.g.E();

            var newContext = context.TransformQuery(_ => otherQuery);

            newContext.Query
                .Should()
                .BeSameAs(otherQuery);

            newContext.ExecutionId
                .Should()
                .Be(context.ExecutionId);
        }

        [Fact]
        public void Uninitialized_Query_throws()
        {
            var context = default(GremlinQueryExecutionContext);

            context.Invoking(c => c.Query)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void Uninitialized_ExecutionId_throws()
        {
            var context = default(GremlinQueryExecutionContext);

            context.Invoking(c => c.ExecutionId)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void Create_throws_on_null()
        {
            var act = () => GremlinQueryExecutionContext.Create(null!);

            act.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void TransformQuery_throws_on_null_transformation()
        {
            var context = GremlinQueryExecutionContext.Create(_query);

            context.Invoking(c => c.TransformQuery(null!))
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}
