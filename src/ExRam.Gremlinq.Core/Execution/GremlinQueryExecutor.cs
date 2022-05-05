using System;
using System.Collections.Generic;
using System.Linq;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core.Execution
{
    public static class GremlinQueryExecutor
    {
        private sealed class GremlinQueryExecutorImpl : IGremlinQueryExecutor
        {
            private readonly Func<ISerializedGremlinQuery, IGremlinQueryEnvironment, IAsyncEnumerable<object>> _factory;

            public GremlinQueryExecutorImpl(Func<ISerializedGremlinQuery, IGremlinQueryEnvironment, IAsyncEnumerable<object>> factory)
            {
                _factory = factory;
            }

            public IAsyncEnumerable<object> Execute(ISerializedGremlinQuery serializedQuery, IGremlinQueryEnvironment environment)
            {
                return _factory(serializedQuery, environment);
            }
        }

        private sealed class TransformQueryGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<ISerializedGremlinQuery, ISerializedGremlinQuery> _transformation;

            public TransformQueryGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor, Func<ISerializedGremlinQuery, ISerializedGremlinQuery> transformation)
            {
                _transformation = transformation;
                _baseExecutor = baseExecutor;
            }

            public IAsyncEnumerable<object> Execute(ISerializedGremlinQuery serializedQuery, IGremlinQueryEnvironment environment)
            {
                return _baseExecutor.Execute(_transformation(serializedQuery), environment);
            }
        }

        private sealed class TransformResultGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<IAsyncEnumerable<object>, IAsyncEnumerable<object>> _transformation;

            public TransformResultGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor, Func<IAsyncEnumerable<object>, IAsyncEnumerable<object>> transformation)
            {
                _transformation = transformation;
                _baseExecutor = baseExecutor;
            }

            public IAsyncEnumerable<object> Execute(ISerializedGremlinQuery serializedQuery, IGremlinQueryEnvironment environment)
            {
                return _transformation(_baseExecutor.Execute(serializedQuery, environment));
            }
        }

        public static readonly IGremlinQueryExecutor Invalid = Create(static (_, _) => throw new InvalidOperationException($"'{nameof(IGremlinQueryExecutor.Execute)}' must not be called on {nameof(GremlinQueryExecutor)}.{nameof(Invalid)}. If you are getting this exception while executing a query, set a proper {nameof(GremlinQueryExecutor)} on the {nameof(GremlinQuerySource)} (e.g. with 'g.UseGremlinServer(...)' for GremlinServer which can be found in the 'ExRam.Gremlinq.Providers.GremlinServer' package)."));

        public static readonly IGremlinQueryExecutor Identity = Create(static (query, _) => new[] { query }.ToAsyncEnumerable());

        public static readonly IGremlinQueryExecutor Empty = Create(static (_, _) => AsyncEnumerable.Empty<object>());

        public static IGremlinQueryExecutor Create(Func<object, IGremlinQueryEnvironment, IAsyncEnumerable<object>> executor) => new GremlinQueryExecutorImpl(executor);

        public static IGremlinQueryExecutor TransformQuery(this IGremlinQueryExecutor baseExecutor, Func<ISerializedGremlinQuery, ISerializedGremlinQuery> transformation) => new TransformQueryGremlinQueryExecutor(baseExecutor, transformation);

        public static IGremlinQueryExecutor TransformResult(this IGremlinQueryExecutor baseExecutor, Func<IAsyncEnumerable<object>, IAsyncEnumerable<object>> transformation) => new TransformResultGremlinQueryExecutor(baseExecutor, transformation);
    }
}
