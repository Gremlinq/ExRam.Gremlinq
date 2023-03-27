namespace ExRam.Gremlinq.Core.Execution
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

            public IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment)
            {
                return _baseExecutor
                    .Execute(query, environment)
                    .IgnoreElements();
            }
        }
        public static IGremlinQueryExecutor IgnoreResults(this IGremlinQueryExecutor executor) => new IgnoringExecutor(executor);
    }
}
