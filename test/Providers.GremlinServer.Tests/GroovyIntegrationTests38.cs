using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/>, every query sent as a Groovy script, against the customized Gremlin Server
    /// 3.8.2. Owns the snapshots of the facts that answer differently than on 3.7 and than <see cref="IntegrationTests38"/>;
    /// <see cref="GroovyIntegrationTests38Base"/> names them.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class GroovyIntegrationTests38 : GroovyIntegrationTests38Base, IClassFixture<GroovyIntegrationTests38.Fixture>, ISourceFileNameProvider<GroovyIntegrationTests38>
    {
        public sealed class Fixture : GroovyGremlinServerContainerFixture
        {
            public Fixture() : base(TinkerPopVersions.V3_8_2)
            {
            }
        }

        public GroovyIntegrationTests38(Fixture fixture) : base(fixture, new ExecutingVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
