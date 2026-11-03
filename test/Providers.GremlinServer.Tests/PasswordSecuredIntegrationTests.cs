using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> against a Gremlin Server that asks for a password, answering like
    /// <see cref="IntegrationTests"/>.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class PasswordSecuredIntegrationTests : IntegrationTestsBase, IClassFixture<PasswordSecuredGremlinServerContainerFixture>
    {
        public PasswordSecuredIntegrationTests(PasswordSecuredGremlinServerContainerFixture fixture) : base(fixture, new ExecutingVerifier())
        {
        }
    }
}
