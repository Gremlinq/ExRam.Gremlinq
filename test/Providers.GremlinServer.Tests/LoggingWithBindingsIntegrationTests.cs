using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public partial class LoggingWithBindingsIntegrationTests : QueryExecutionTest, IClassFixture<LoggingWithBindingsIntegrationTests.LoggingFixture>
    {
        [GeneratedRegex("^Executing Gremlin query 12345678-9012-3456-7890-123456789012.*")]
        private static partial Regex ExecutingMessageRegex();

        public class LoggingFixture : LoggingIntegrationTests.LoggingFixture
        {
            public override IGremlinQuerySource GetQuerySource() => base
                .GetQuerySource()
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(_ => _
                        .SetValue(GremlinqOption.QueryLogVerbosity, QueryLogVerbosity.IncludeBindings)));
        }

        public class LoggingVerifier : LoggingIntegrationTests.LoggingVerifier
        {
            public LoggingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task
                    .ScrubRegex(ExecutingMessageRegex(), "(Message)"));
        }

        public LoggingWithBindingsIntegrationTests(LoggingFixture fixture) : base(fixture, new LoggingVerifier())
        {
        }
    }
}
