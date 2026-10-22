using ExRam.Gremlinq.Core.Execution;

using FluentAssertions;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryExtensionsTest
    {
        [Fact]
        public async Task ToArrayAsync_returns_all_elements() => (await Query(1, 2)
            .ToArrayAsync(TestContext.Current.CancellationToken))
            .Should()
            .Equal(1, 2);

        [Fact]
        public async Task ToArrayAsync_returns_empty_array() => (await Query()
            .ToArrayAsync(TestContext.Current.CancellationToken))
            .Should()
            .BeEmpty();

        [Fact]
        public async Task FirstAsync_returns_first_element() => (await Query(1, 2)
            .FirstAsync(TestContext.Current.CancellationToken))
            .Should()
            .Be(1);

        [Fact]
        public Task FirstAsync_throws_on_empty_sequence() => Query()
            .Awaiting(query => query
                .FirstAsync(TestContext.Current.CancellationToken))
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task FirstOrDefaultAsync_returns_first_element() => (await Query(1, 2)
            .FirstOrDefaultAsync(TestContext.Current.CancellationToken))
            .Should()
            .Be(1);

        [Fact]
        public async Task FirstOrDefaultAsync_returns_default_on_empty_sequence() => (await Query()
            .FirstOrDefaultAsync(TestContext.Current.CancellationToken))
            .Should()
            .Be(0);

        [Fact]
        public async Task SingleAsync_returns_only_element() => (await Query(1)
            .SingleAsync(TestContext.Current.CancellationToken))
            .Should()
            .Be(1);

        [Fact]
        public Task SingleAsync_throws_on_empty_sequence() => Query()
            .Awaiting(query => query
                .SingleAsync(TestContext.Current.CancellationToken))
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public Task SingleAsync_throws_on_more_than_one_element() => Query(1, 2)
            .Awaiting(query => query
                .SingleAsync(TestContext.Current.CancellationToken))
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task SingleOrDefaultAsync_returns_only_element() => (await Query(1)
            .SingleOrDefaultAsync(TestContext.Current.CancellationToken))
            .Should()
            .Be(1);

        [Fact]
        public async Task SingleOrDefaultAsync_returns_default_on_empty_sequence() => (await Query()
            .SingleOrDefaultAsync(TestContext.Current.CancellationToken))
            .Should()
            .Be(0);

        [Fact]
        public Task SingleOrDefaultAsync_throws_on_more_than_one_element() => Query(1, 2)
            .Awaiting(query => query
                .SingleOrDefaultAsync(TestContext.Current.CancellationToken))
            .Should()
            .ThrowAsync<InvalidOperationException>();

        private static IGremlinQuery<int> Query(params int[] results) => g
            .ConfigureEnvironment(env => env
                .UseExecutor(new ResultsExecutor(results)))
            .Inject(0);

        private sealed class ResultsExecutor : IGremlinQueryExecutor
        {
            private readonly int[] _results;

            public ResultsExecutor(int[] results)
            {
                _results = results;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context) => _results
                .Cast<T>()
                .ToAsyncEnumerable();
        }
    }
}
