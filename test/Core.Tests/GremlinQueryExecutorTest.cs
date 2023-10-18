using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

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

        [Fact]
        public async Task Serialize()
        {
            var state = 0;
            var random = new Random(DateTime.UtcNow.Millisecond);
            var baseExecutor = Substitute.For<IGremlinQueryExecutor>();

            baseExecutor
                .Execute<int>(Arg.Any<GremlinQueryExecutionContext>())
                .Returns(Core());

            var serialized = baseExecutor
                .Serialize();

            await Observable
                .Range(1, 1000)
                .SelectMany(x => serialized
                    .Execute<int>(GremlinQueryExecutionContext.Create(_query))
                    .ToObservable())
                .LastOrDefaultAsync()
                .ToTask();
                    
            async IAsyncEnumerable<int> Core()
            {
                Interlocked.CompareExchange(ref state, 1, 0)
                    .Should()
                    .Be(0);

                yield return 42;
                await Task.Delay(TimeSpan.FromMilliseconds(random.Next(5)));

                Interlocked.CompareExchange(ref state, 0, 1)
                    .Should()
                    .Be(1);
            }
        }
    }
}
