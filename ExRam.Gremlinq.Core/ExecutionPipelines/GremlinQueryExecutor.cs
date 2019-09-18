using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutor
    {
        private sealed class EchoGremlinQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<object> Execute(object serializedQuery)
            {
                return AsyncEnumerableEx.Return(serializedQuery);
            }
        }

        private sealed class InvalidQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<object> Execute(object serializedQuery)
            {
                return AsyncEnumerableEx.Throw<object>(new InvalidOperationException($"'{nameof(Execute)}' must not be called on {nameof(GremlinQueryExecutor)}.Invalid. If you are getting this exception while executing a query, set a proper {nameof(GremlinQueryExecutor)} on the {nameof(GremlinQuerySource)} (e.g. with 'g.WithRemote(...)' for WebSockets which can be found in the 'ExRam.Gremlinq.Providers.WebSocket' package)."));
            }
        }

        public static readonly IGremlinQueryExecutor Invalid = new InvalidQueryExecutor();

        public static readonly IGremlinQueryExecutor Echo = new EchoGremlinQueryExecutor();
    }
}
