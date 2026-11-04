using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

using VerifyTests.MicrosoftLogging;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// Verifies what the provider logs while a query executes on <see cref="LoggingGremlinServerContainerFixture"/>, not its result.
    /// </summary>
    public sealed class LoggingVerifier : GremlinQueryVerifier
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
}
