using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutor
    {
        private sealed class GremlinQueryExecutorImpl : IGremlinQueryExecutor
        {
            private readonly Func<object, IGremlinQueryEnvironment, IAsyncEnumerable<object>> _factory;

            public GremlinQueryExecutorImpl(Func<object, IGremlinQueryEnvironment, IAsyncEnumerable<object>> factory)
            {
                _factory = factory;
            }

            public IAsyncEnumerable<object> Execute(object serializedQuery, IGremlinQueryEnvironment environment)
            {
                return _factory(serializedQuery, environment);
            }
        }

        private sealed class QueryInterceptingGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<object, object> _transformation;

            public QueryInterceptingGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor, Func<object, object> transformation)
            {
                _transformation = transformation;
                _baseExecutor = baseExecutor;
            }

            public IAsyncEnumerable<object> Execute(object serializedQuery, IGremlinQueryEnvironment environment)
            {
                return _baseExecutor.Execute(_transformation(serializedQuery), environment);
            }
        }

        private sealed class ResultInterceptingGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<IAsyncEnumerable<object>, IAsyncEnumerable<object>> _transformation;

            public ResultInterceptingGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor, Func<IAsyncEnumerable<object>, IAsyncEnumerable<object>> transformation)
            {
                _transformation = transformation;
                _baseExecutor = baseExecutor;
            }

            public IAsyncEnumerable<object> Execute(object serializedQuery, IGremlinQueryEnvironment environment)
            {
                return _transformation(_baseExecutor.Execute(serializedQuery, environment));
            }
        }

        public static readonly IGremlinQueryExecutor Invalid = Create((query, env) => AsyncEnumerableEx.Throw<object>(new InvalidOperationException($"'{nameof(IGremlinQueryExecutor.Execute)}' must not be called on {nameof(GremlinQueryExecutor)}.{nameof(Invalid)}. If you are getting this exception while executing a query, set a proper {nameof(GremlinQueryExecutor)} on the {nameof(GremlinQuerySource)} (e.g. with 'g.UseGremlinServer(...)' for GremlinServer which can be found in the 'ExRam.Gremlinq.Providers.UseGremlinServer' package).")));

        public static readonly IGremlinQueryExecutor Identity = Create((query, env) => AsyncEnumerableEx.Return(query));

        public static readonly IGremlinQueryExecutor Empty = Create((query, env) => AsyncEnumerable.Empty<object>());

        public static IGremlinQueryExecutor Create(Func<object, IGremlinQueryEnvironment, IAsyncEnumerable<object>> executor) => new GremlinQueryExecutorImpl(executor);

        public static IGremlinQueryExecutor InterceptQuery(this IGremlinQueryExecutor baseExecutor, Func<object, object> transformation) => new QueryInterceptingGremlinQueryExecutor(baseExecutor, transformation);

        public static IGremlinQueryExecutor InterceptResult(this IGremlinQueryExecutor baseExecutor, Func<IAsyncEnumerable<object>, IAsyncEnumerable<object>> transformation) => new ResultInterceptingGremlinQueryExecutor(baseExecutor, transformation);
    }
}
