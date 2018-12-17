using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    internal static class GremlinQueryProvider
    {
        private sealed class InvalidQueryExecutor : IGremlinQueryExecutor
        {
            public bool SupportsElementType(Type type) => false;

            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return AsyncEnumerable.Throw<TElement>(new InvalidOperationException());
            }
        }

        public static readonly IGremlinQueryExecutor Invalid = new InvalidQueryExecutor();
    }
}
