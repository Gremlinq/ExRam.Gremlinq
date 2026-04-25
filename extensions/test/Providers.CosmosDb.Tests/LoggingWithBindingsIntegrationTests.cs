using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    [IntegrationTest("Windows", true)]
    public class LoggingWithBindingsIntegrationTests : CosmosDbIntegrationTestBase, IClassFixture<LoggingWithBindingsIntegrationTests.LoggingFixture>
    {
        public class LoggingFixture : LoggingIntegrationTests.LoggingFixture
        {
            public override IGremlinQuerySource GetQuerySource() => base
                .GetQuerySource()
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(_ => _
                        .SetValue(GremlinqOption.QueryLogVerbosity, QueryLogVerbosity.IncludeBindings)));
        }

        public LoggingWithBindingsIntegrationTests(LoggingFixture fixture) : base(fixture, new LoggingIntegrationTests.LoggingVerifier())
        {
        }
    }
}
