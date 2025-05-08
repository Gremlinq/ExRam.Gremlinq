using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

using VerifyTests.MicrosoftLogging;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class LoggingWithBindingsIntegrationTests : LoggingIntegrationTests, IClassFixture<LoggingWithBindingsIntegrationTests.LoggingFixture>
    {
        public new class LoggingFixture : LoggingIntegrationTests.LoggingFixture
        {
            private readonly RecordingProvider _recordingProvider = new();

            public override IGremlinQuerySource GetQuerySource()
            {
                return base
                    .GetQuerySource()
                    .ConfigureEnvironment(env => env
                        .ConfigureLogger(_ => _recordingProvider.CreateLogger<LoggingFixture>()));
            }
        }

        public LoggingWithBindingsIntegrationTests(LoggingFixture fixture) : base(fixture)
        {
        }
    }
}
