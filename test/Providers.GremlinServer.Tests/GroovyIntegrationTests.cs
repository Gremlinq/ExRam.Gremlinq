using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/>, every query sent as a Groovy script, against the customized Gremlin Server
    /// 3.7.7. Owns the snapshots of the facts that the script engine answers differently; <see cref="GroovyIntegrationTestsBase"/>
    /// names them.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class GroovyIntegrationTests : GroovyIntegrationTestsBase, IClassFixture<GroovyGremlinServerContainerFixture>, ISourceFileNameProvider<GroovyIntegrationTests>
    {
        public GroovyIntegrationTests(GroovyGremlinServerContainerFixture fixture) : base(fixture, new ExecutingVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
