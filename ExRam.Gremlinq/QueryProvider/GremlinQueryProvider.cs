using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq
{
    internal static class GremlinQueryProvider
    {
        private sealed class InvalidQueryProvider : IGremlinQueryProvider
        {
            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return AsyncEnumerable.Throw<TElement>(new InvalidOperationException());
            }
        }

        public static readonly IGremlinQueryProvider Invalid = new InvalidQueryProvider();
    }
}
