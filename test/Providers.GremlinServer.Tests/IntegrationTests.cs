using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;
using FluentAssertions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public IntegrationTests(GremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new ExecutingVerifier(),
            testOutputHelper)
        {
        }

        [Fact]
        public async Task SingleAsync_throws_on_too_many_values()
        {
            await _g
                .Inject(1, 2)
                .Awaiting(_ => _
                    .SingleAsync())
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task SingleAsync_throws_on_too_few_values()
        {
            await _g
                .Inject(1)
                .None()
                .Awaiting(_ => _
                    .SingleAsync())
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task SingleOrDefaultAsync_returns_default_on_too_many_values()
        {
            var result = await _g
                .Inject(1, 2)
                .SingleOrDefaultAsync();

            result
                .Should()
                .Be(0);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_throws_on_too_few_values()
        {
            var result = await _g
                .Inject(1)
                .None()
                .Awaiting(_ => _
                    .SingleOrDefaultAsync())
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }
    }
}
