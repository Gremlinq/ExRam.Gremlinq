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

        public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(Execute(query));

        protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
            .ModifySettingsTask(task)
            .DontScrubDateTimes()
            .DontIgnoreEmptyCollections()
            .DontScrubGuids()
            .ScrubGuidsWithConstant()
            .IgnoreStackTrace()
            .IgnoreMember("stackTrace");

        private static async Task<object> Execute<TElement>(IGremlinQueryBase<TElement> query)
        {
            try
            {
                return await query
                    .ToAsyncEnumerable()
                    .ToArrayAsync();
            }
            catch (GremlinQueryExecutionException ex)
            {
                return ex;
            }
        }
    }
}
