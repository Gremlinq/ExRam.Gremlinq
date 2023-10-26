using ExRam.Gremlinq.Core.Execution;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public static class GremlinQueryExecutorExtensions
    {
        private sealed class IgnoringExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;

            public IgnoringExecutor(IGremlinQueryExecutor baseExecutor)
            {
                _baseExecutor = baseExecutor;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                return _baseExecutor
                    .Execute<T>(context)
                    .IgnoreElements();
            }
        }

        public static IGremlinQueryExecutor IgnoreResults(this IGremlinQueryExecutor executor) => new IgnoringExecutor(executor);
    }
}
