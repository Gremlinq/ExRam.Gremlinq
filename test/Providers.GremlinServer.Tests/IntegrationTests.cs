using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;
using FluentAssertions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest]
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        public IntegrationTests(GremlinServerContainerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new JTokenExecutingVerifier(),
            testOutputHelper)
        {
        }

        [Fact]
        public async Task FirstAsync() => (await _g
            .Inject(42)
            .FirstAsync())
                .Should()
                .Be(42);

        [Fact]
        public async Task FirstAsync_empty() => await _g
            .Inject(42)
            .None()
            .Awaiting(_ => _
                .FirstAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task FirstOrDefaultAsync() => (await _g
            .Inject(42)
            .FirstOrDefaultAsync())
                .Should()
                .Be(42);

        [Fact]
        public async Task FirstOrDefaultAsync_empty() => (await _g
            .Inject(42)
            .None()
            .FirstOrDefaultAsync())
                .Should()
                .Be(0);

        [Fact]
        public async Task SingleAsync() => (await _g
           .Inject(42)
           .SingleAsync())
               .Should()
               .Be(42);

        [Fact]
        public async Task SingleAsync_empty() => await _g
            .Inject(42)
            .None()
            .Awaiting(_ => _
                .SingleAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task SingleAsync_two_elements() => await _g
            .Inject(42, 43)
            .Awaiting(_ => _
                .SingleAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task SingleOrDefaultAsync() => (await _g
            .Inject(42)
            .SingleOrDefaultAsync())
                .Should()
                .Be(42);

        [Fact]
        public async Task SingleOrDefaultAsync_empty() => (await _g
            .Inject(42)
            .None()
            .SingleOrDefaultAsync())
                .Should()
                .Be(0);

        [Fact]
        public async Task SingleOrDefaultAsync_two_elements() => await _g
            .Inject(42, 43)
            .Awaiting(_ => _
                .SingleOrDefaultAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task LastAsync() => (await _g
           .Inject(42, 43)
           .LastAsync())
               .Should()
               .Be(43);

        [Fact]
        public async Task LastAsync_empty() => await _g
            .Inject(42)
            .None()
            .Awaiting(_ => _
                .LastAsync())
            .Should()
            .ThrowAsync<InvalidOperationException>();

        [Fact]
        public async Task LastOrDefaultAsync() => (await _g
            .Inject(42, 43)
            .LastOrDefaultAsync())
                .Should()
                .Be(43);

        [Fact]
        public async Task LastOrDefaultAsync_empty() => (await _g
            .Inject(42)
            .None()
            .LastOrDefaultAsync())
                .Should()
                .Be(0);
    }
}
