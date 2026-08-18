using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

using ExRam.Gremlinq.Core.Execution;

using FluentAssertions;

using NSubstitute;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryExecutorTest
    {
        private readonly IVertexGremlinQuery<object> _query;

        public GremlinQueryExecutorTest()
        {
            _query = GremlinQuerySource.g.V();
        }

        [Fact]
        public void Invalid() => GremlinQueryExecutor.Invalid
            .Execute<object>(GremlinQueryExecutionContext.Create(_query))
            .Awaiting(ex => ex
                .ToArrayAsync(TestContext.Current.CancellationToken))
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task Empty()
        {
            var results = await GremlinQueryExecutor.Empty
                .Execute<object>(GremlinQueryExecutionContext.Create(_query))
                .ToArrayAsync(TestContext.Current.CancellationToken);

            results
                .Should()
                .BeEmpty();
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
                    .ToArrayAsync(TestContext.Current.CancellationToken))
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
                .SelectMany(_ => serialized
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
                await Task.Delay(TimeSpan.FromMilliseconds(random.Next(5)), TestContext.Current.CancellationToken);

                Interlocked.CompareExchange(ref state, 0, 1)
                    .Should()
                    .Be(1);
            }
        }

        [Fact]
        public async Task TransformQuery()
        {
            var transformCalled = false;
            var baseExecutor = Substitute.For<IGremlinQueryExecutor>();

            baseExecutor
                .Execute<int>(Arg.Any<GremlinQueryExecutionContext>())
                .Returns(new[] { 1, 2, 3 }.ToAsyncEnumerable());

            var results = await baseExecutor
                .TransformQuery(query =>
                {
                    transformCalled = true;
                    return query;
                })
                .Execute<int>(GremlinQueryExecutionContext.Create(_query))
                .ToArrayAsync(TestContext.Current.CancellationToken);

            transformCalled
                .Should()
                .BeTrue();

            results
                .Should()
                .Equal(1, 2, 3);
        }

        [Fact]
        public async Task TransformQuery_disposes_base_executor_if_IAsyncDisposable()
        {
            var baseExecutor = Substitute.For<IGremlinQueryExecutor, IAsyncDisposable>();

            var executor = baseExecutor
                .TransformQuery(_ => _);

            executor
                .Should()
                .BeAssignableTo<IAsyncDisposable>();

            await ((IAsyncDisposable)executor).DisposeAsync();

            await ((IAsyncDisposable)baseExecutor)
                .Received(1)
                .DisposeAsync();
        }

        [Fact]
        public async Task TransformQuery_dispose_without_IAsyncDisposable_base_does_not_throw()
        {
            var baseExecutor = Substitute.For<IGremlinQueryExecutor>();

            var executor = baseExecutor
                .TransformQuery(_ => _);

            await ((IAsyncDisposable)executor).DisposeAsync();
        }

        [Fact]
        public async Task TransformExecutionException_disposes_base_executor_if_IAsyncDisposable()
        {
            var baseExecutor = Substitute.For<IGremlinQueryExecutor, IAsyncDisposable>();

            var executor = baseExecutor
                .TransformExecutionException(ex => ex);

            executor
                .Should()
                .BeAssignableTo<IAsyncDisposable>();

            await ((IAsyncDisposable)executor).DisposeAsync();

            await ((IAsyncDisposable)baseExecutor)
                .Received(1)
                .DisposeAsync();
        }

        [Fact]
        public async Task TransformExecutionException_dispose_without_IAsyncDisposable_base_does_not_throw()
        {
            var baseExecutor = Substitute.For<IGremlinQueryExecutor>();

            var executor = baseExecutor
                .TransformExecutionException(ex => ex);

            await ((IAsyncDisposable)executor).DisposeAsync();
        }

        [Fact]
        public async Task Serialize_disposes_base_executor_if_IAsyncDisposable()
        {
            var baseExecutor = Substitute.For<IGremlinQueryExecutor, IAsyncDisposable>();

            var executor = baseExecutor
                .Serialize();

            executor
                .Should()
                .BeAssignableTo<IAsyncDisposable>();

            await ((IAsyncDisposable)executor).DisposeAsync();

            await ((IAsyncDisposable)baseExecutor)
                .Received(1)
                .DisposeAsync();
        }

        [Fact]
        public async Task Serialize_dispose_without_IAsyncDisposable_base_does_not_throw()
        {
            var baseExecutor = Substitute.For<IGremlinQueryExecutor>();

            var executor = baseExecutor
                .Serialize();

            await ((IAsyncDisposable)executor).DisposeAsync();
        }

        [Fact]
        public async Task Executor_from_environment_disposes_wrapped_base_executor()
        {
            var baseExecutor = Substitute.For<IGremlinQueryExecutor, IAsyncDisposable>();

            var environment = GremlinQueryEnvironment.Invalid
                .UseExecutor(baseExecutor
                    .TransformExecutionException(ex => ex));

            if (environment.Executor is IAsyncDisposable asyncDisposable)
                await asyncDisposable.DisposeAsync();

            await ((IAsyncDisposable)baseExecutor)
                .Received(1)
                .DisposeAsync();
        }

        [Fact]
        public void TransformQuery_throws_on_null_executor()
        {
            var act = () => GremlinQueryExecutor.TransformQuery(null!, _ => _);

            act.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void TransformQuery_throws_on_null_transformation()
        {
            var executor = Substitute.For<IGremlinQueryExecutor>();

            var act = () => executor.TransformQuery(null!);

            act.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Serialize_throws_on_null()
        {
            var act = () => GremlinQueryExecutor.Serialize(null!);

            act.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void TransformExecutionException_throws_on_null_executor()
        {
            var act = () => GremlinQueryExecutor.TransformExecutionException(null!, ex => ex);

            act.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void TransformExecutionException_throws_on_null_transformation()
        {
            var executor = Substitute.For<IGremlinQueryExecutor>();

            var act = () => executor.TransformExecutionException(null!);

            act.Should()
                .Throw<ArgumentNullException>();
        }
    }
}
