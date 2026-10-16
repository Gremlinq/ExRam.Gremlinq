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
        public class LoggingFixture : GremlinServerContainerFixture
        {
            public override IGremlinQuerySource GetQuerySource() => base
                .GetQuerySource()
                .ConfigureEnvironment(env => env
                    .ConfigureLogger(_ => RecordingProvider.CreateLogger<LoggingFixture>()));
        }

        public class LoggingVerifier : GremlinQueryVerifier
        {
            public LoggingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(Record(query));

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .DontScrubGuids()
                .ScrubGuidsWithConstant()
                .IgnoreStackTrace()
                .IgnoreMember("stackTrace");

            private static async Task<IReadOnlyCollection<ToAppend>> Record<TElement>(IGremlinQueryBase<TElement> query)
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

                return Recording.Stop();
            }
        }

        public LoggingIntegrationTests(LoggingFixture fixture) : base(
            fixture,
            new LoggingVerifier())
        {
        }
    }
}
