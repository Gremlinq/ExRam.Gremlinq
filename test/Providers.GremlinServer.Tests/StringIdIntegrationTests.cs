using System.Globalization;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Support.TestContainers;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class StringIdIntegrationTests : QueryExecutionTestsBase, IClassFixture<StringIdIntegrationTests.Fixture>
    {
        // The gremlin-server-mod image answers with string ids. The executor ignores the results, so the facts only prove
        // that their queries execute with such ids.
        public sealed class Fixture : GremlinServerContainerFixture
        {
            protected override IGremlinServerConfigurator ConfigureProvider(IGremlinServerConfigurator configurator) => configurator
                .UseGremlinServerModContainer("3.7.7");

            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => base
                .TransformQuerySource(g)
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutor(_ => _
                        .IgnoreResults()));
        }

        public StringIdIntegrationTests(Fixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }

        protected override object GetId(long n) => n.ToString(CultureInfo.InvariantCulture);
    }
}
