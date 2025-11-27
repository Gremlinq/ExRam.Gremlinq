using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class LoggingWithBindingsIntegrationTests : QueryExecutionTest, IClassFixture<LoggingWithBindingsIntegrationTests.LoggingFixture>
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
