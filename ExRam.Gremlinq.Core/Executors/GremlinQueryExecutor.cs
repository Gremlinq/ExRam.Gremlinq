using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryExecutor
    {
        private sealed class InvalidQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return AsyncEnumerableEx.Throw<TElement>(new InvalidOperationException($"'{nameof(Execute)}' must not be called on GremlinQueryExecutor.Invalid. If you are getting this exception while executing a query, set a proper GremlinQueryExecutor on the GremlinQuerySource (e.g. with 'g.WithRemote(...)' for WebSockets)."));
            }
        }

        public static readonly IGremlinQueryExecutor Invalid = new InvalidQueryExecutor();
    }
}
