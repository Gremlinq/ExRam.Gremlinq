using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using FluentAssertions;
using ExRam.Gremlinq.Core.Execution;
using Gremlin.Net.Driver.Exceptions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux")]
    [IntegrationTest("Windows")]
    public class WrongPasswordIntegrationTests : GremlinqTestBase, IClassFixture<WrongPasswordGremlinServerContainerFixture>
    {
        private readonly IGremlinQuerySource _g;

        public WrongPasswordIntegrationTests(WrongPasswordGremlinServerContainerFixture fixture) : base(new ExecutingVerifier())
        {
            _g = fixture.G;
        }

        [Fact]
        public async Task Wrong_password_bubbles_up()
        {
            await _g
                .Inject(42)
                .ToArrayAsync()
                .Awaiting(_ => _)
                .Should()
                .ThrowAsync<GremlinQueryExecutionException>()
                .WithInnerException<GremlinQueryExecutionException, ResponseException>()
                .WithMessage("Unauthorized: Username and/or password are incorrect");
        }
    }
}
