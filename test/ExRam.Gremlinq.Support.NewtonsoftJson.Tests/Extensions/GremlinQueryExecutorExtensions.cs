using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core.Execution
{
    internal static class GremlinQueryExecutorExtensions
    {
        private sealed class CatchingGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;

            public CatchingGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor)
            {
                _baseExecutor = baseExecutor;
            }

            public IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment) => _baseExecutor
                .Execute(query, environment)
                .Catch<object, Exception>(ex => AsyncEnumerableEx
                    .Return<object>(new JObject()
                    {
                        {
                            "serverException",
                            new JObject
                            {
                                { "type", ex.GetType().Name },
                                { "message", ex.Message }
                            }
                        }
                    }));
        }

        public static IGremlinQueryExecutor CatchExecutionExceptions(this IGremlinQueryExecutor executor) => new CatchingGremlinQueryExecutor(executor);
    }
}
