using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using VerifyTests.MicrosoftLogging;

using static ExRam.Gremlinq.Providers.CosmosDb.Tests.IntegrationTests;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    [IntegrationTest("Windows", true)]
    public class LoggingIntegrationTests : CosmosDbIntegrationTestBase, IClassFixture<LoggingIntegrationTests.LoggingFixture>
    {
        public class LoggingFixture : CosmosDbEmulatorFixture
        {
            private readonly RecordingProvider _recordingProvider = new();

            public override IGremlinQuerySource GetQuerySource() => base
                .GetQuerySource()
                .ConfigureEnvironment(env => env
                    .ConfigureLogger(_ => _recordingProvider.CreateLogger<LoggingFixture>()));
        }

        public class LoggingVerifier : CosmosDbEmulatorExecutingVerifier
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
                .ScrubGuidsWithConstant()
                .ScrubLinesWithReplace(str => str.Contains("and parameter bindings ")
                    ? "Query with instable bindings"
                    : str);
        }

        public LoggingIntegrationTests(LoggingFixture fixture) : base(
            fixture,
            new LoggingVerifier())
        {
        }
    }
}
