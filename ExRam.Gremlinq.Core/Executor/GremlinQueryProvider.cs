using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQueryProvider
    {
        private sealed class InvalidQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return AsyncEnumerable.Throw<TElement>(new InvalidOperationException());
            }
        }

        public static readonly IGremlinQueryExecutor Invalid = new InvalidQueryExecutor();
    }
}
