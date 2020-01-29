using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutor
    {
        private sealed class GremlinQueryExecutorImpl : IGremlinQueryExecutor
        {
            private readonly Func<object, IAsyncEnumerable<object>> _factory;

            public GremlinQueryExecutorImpl(Func<object, IAsyncEnumerable<object>> factory)
            {
                _factory = factory;
            }

            public IAsyncEnumerable<object> Execute(object serializedQuery)
            {
                return _factory(serializedQuery);
            }
        }

        public static readonly IGremlinQueryExecutor Invalid = new GremlinQueryExecutorImpl(_ => AsyncEnumerableEx.Throw<object>(new InvalidOperationException($"'{nameof(IGremlinQueryExecutor.Execute)}' must not be called on {nameof(GremlinQueryExecutor)}.{nameof(Invalid)}. If you are getting this exception while executing a query, set a proper {nameof(GremlinQueryExecutor)} on the {nameof(GremlinQuerySource)} (e.g. with 'g.UseGremlinServer(...)' for GremlinServer which can be found in the 'ExRam.Gremlinq.Providers.UseGremlinServer' package).")));

        public static readonly IGremlinQueryExecutor Echo = new GremlinQueryExecutorImpl(AsyncEnumerableEx.Return);

        public static readonly IGremlinQueryExecutor Empty = new GremlinQueryExecutorImpl(_ => AsyncEnumerable.Empty<object>());
    }
}
