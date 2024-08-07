using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class ExecutingVerifier : GremlinQueryVerifier
    {
        public ExecutingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
        {
        }

        public override async Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            try
            {
                await InnerVerify(await query
                    .ToAsyncEnumerable()
                    .ToArrayAsync());
            }
            catch (GremlinQueryExecutionException)
            {
                await InnerVerify(Array.Empty<TElement>());    //TODO: Verify exception
            }
        }

        protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
            .ModifySettingsTask(task)
            .DontScrubDateTimes()
            .DontIgnoreEmptyCollections()
            .DontScrubGuids()
            .ScrubGuids();
    }
}
