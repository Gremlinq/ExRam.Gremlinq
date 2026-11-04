using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class LoggingWithBindingsIntegrationTests : QueryExecutionTestsBase, IClassFixture<LoggingWithBindingsIntegrationTests.Fixture>
    {
        public sealed class Fixture : LoggingGremlinServerContainerFixture
        {
            public override IGremlinQuerySource GetQuerySource() => base
                .GetQuerySource()
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(_ => _
                        .SetValue(GremlinqOption.QueryLogVerbosity, QueryLogVerbosity.IncludeBindings)));
        }

        public LoggingWithBindingsIntegrationTests(Fixture fixture) : base(fixture, new LoggingVerifier())
        {
        }
    }
}
