using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionResultDeserializer<TExecutionResult>
    {
        private sealed class InvalidQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer<TExecutionResult>
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(TExecutionResult result)
            {
                return AsyncEnumerableEx.Throw<TElement>(new InvalidOperationException($"'{nameof(Deserialize)}' must not be called on GremlinQueryExecutionResultDeserializer.Invalid. If you are getting this exception while executing a result, set a proper GremlinQueryExecutor on the GremlinQuerySource (e.g. with 'g.WithRemote(...)' for WebSockets)."));   //TODO: Refine message
            }
        }

        public static readonly IGremlinQueryExecutionResultDeserializer<TExecutionResult> Invalid = new InvalidQueryExecutionResultDeserializer();
    }
}
