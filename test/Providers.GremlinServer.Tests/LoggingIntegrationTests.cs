using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using VerifyTests.MicrosoftLogging;

using static ExRam.Gremlinq.Providers.GremlinServer.Tests.LoggingIntegrationTests;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class LoggingIntegrationTests : QueryExecutionTest, IClassFixture<LoggingFixture>
    {
        public sealed class LoggingFixture : GremlinServerContainerFixture
        {
            private readonly RecordingProvider _recordingProvider = new ();

            public override IGremlinQuerySource GetQuerySource()
            {
                return base
                    .GetQuerySource()
                    .ConfigureEnvironment(env => env
                        .ConfigureLogger(_ => _recordingProvider.CreateLogger<LoggingFixture>()));
            }
        }

        public class LoggingVerifier : GremlinQueryVerifier
        {
            public LoggingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            public override async Task Verify<TElement>(IGremlinQueryBase<TElement> query)
            {
                Recording.Start();

                try
                {
                    await query
                        .ToAsyncEnumerable()
                        .ToArrayAsync();
                }
                catch
                {
                }
                finally
                {
                    await InnerVerify(Recording.Stop());
                }
            }

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .DontScrubGuids()
                .ScrubGuids();
        }

        public LoggingIntegrationTests(LoggingFixture fixture) : base(
            fixture,
            new LoggingVerifier())
        {
        }
    }
}
