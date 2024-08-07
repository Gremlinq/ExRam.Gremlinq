using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class ExecutingVerifier : GremlinQueryVerifier
    {
        public ExecutingVerifier(Func<SettingsTask, SettingsTask>? settingsTaskModifier = null, [CallerFilePath] string sourceFile = "") : base(settingsTaskModifier, sourceFile)
        {
        }

        public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(Execute(query));

        private async ValueTask<TElement[]> Execute<TElement>(IGremlinQueryBase<TElement> query)
        {
            try
            {
                return await query
                    .ToAsyncEnumerable()
                    .ToArrayAsync();
            }
            catch (GremlinQueryExecutionException)
            {
                return Array.Empty<TElement>();
            }
        }

        protected override SettingsTask InnerVerify<T>(ValueTask<T> value) => base
            .InnerVerify(value)
            .DontScrubDateTimes()
            .DontIgnoreEmptyCollections()
            .DontScrubGuids()
            .ScrubGuids();
    }
}
