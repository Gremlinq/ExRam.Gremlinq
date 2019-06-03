using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    internal sealed class GremlinQueryExecutor<TSerializedQuery, TExecutionResult>
    {
        private sealed class InvalidQueryExecutor : IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>
        {
            public IAsyncEnumerable<TExecutionResult> Execute(TSerializedQuery serializedQuery)
            {
                return AsyncEnumerableEx.Throw<TExecutionResult>(new InvalidOperationException($"'{nameof(Execute)}' must not be called on GremlinQueryExecutor.Invalid. If you are getting this exception while executing a query, set a proper GremlinQueryExecutor on the GremlinQuerySource (e.g. with 'g.WithRemote(...)' for WebSockets).")); //TODO: Review message.
            }
        }

        public static readonly IGremlinQueryExecutor<TSerializedQuery, TExecutionResult> Invalid = new InvalidQueryExecutor();
    }
}
