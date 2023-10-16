using ExRam.Gremlinq.Core.Execution;

using FluentAssertions;

using NSubstitute;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryExecutorTest : VerifyBase
    {
        private readonly IVertexGremlinQuery<object> _query;

        public GremlinQueryExecutorTest() : base()
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

        [Fact]
        public async Task RetryWithExponentialBackoff()
        {
            var baseExecutor = Substitute.For<IGremlinQueryExecutor>();

            baseExecutor
                .Execute<object>(Arg.Any<GremlinQueryExecutionContext>())
                .Returns(
                    _ => AsyncEnumerableEx.Throw<object>(new GremlinQueryExecutionException(_.Arg<GremlinQueryExecutionContext>(), new DivideByZeroException())),
                    _ => AsyncEnumerableEx.Return<object>("Result"));

            await Verify(baseExecutor
                .RetryWithExponentialBackoff((_, ex) => true)
                .Execute<object>(GremlinQueryExecutionContext.Create(_query))
                .ToArrayAsync());
        }

        [Fact]
        public async Task TransformExecutionException()
        {
            var baseExecutor = Substitute.For<IGremlinQueryExecutor>();

            baseExecutor
                .Execute<object>(Arg.Any<GremlinQueryExecutionContext>())
                .Returns(
                    _ => AsyncEnumerableEx.Throw<object>(new GremlinQueryExecutionException(_.Arg<GremlinQueryExecutionContext>(), new DivideByZeroException())));

            await baseExecutor
                .TransformExecutionException(ex =>
                {
                    ex.InnerException
                        .Should()
                        .BeOfType<DivideByZeroException>();

                    return new GremlinQueryExecutionException(ex.ExecutionContext, new ArrayTypeMismatchException());
                })
                .Execute<object>(GremlinQueryExecutionContext.Create(_query))
                .Awaiting(x => x
                    .ToArrayAsync())
                .Should()
                .ThrowAsync<GremlinQueryExecutionException>()
                .WithInnerException<GremlinQueryExecutionException, ArrayTypeMismatchException>();

        }
    }
}
