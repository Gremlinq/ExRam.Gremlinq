using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Execution
{
    public static class GremlinQueryExecutor
    {
        private sealed class GremlinQueryExecutorImpl : IGremlinQueryExecutor
        {
            private readonly Func<IGremlinQueryBase, IGremlinQueryEnvironment, IAsyncEnumerable<object>> _factory;

            public GremlinQueryExecutorImpl(Func<IGremlinQueryBase, IGremlinQueryEnvironment, IAsyncEnumerable<object>> factory)
            {
                _factory = factory;
            }

            public IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment)
            {
                return _factory(query, environment);
            }
        }

        private sealed class TransformQueryGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<IGremlinQueryBase, IGremlinQueryBase> _transformation;

            public TransformQueryGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor, Func<IGremlinQueryBase, IGremlinQueryBase> transformation)
            {
                _transformation = transformation;
                _baseExecutor = baseExecutor;
            }

            public IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment)
            {
                return _baseExecutor.Execute(_transformation(query), environment);
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

            public IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment)
            {
                return _transformation(_baseExecutor.Execute(query, environment));
            }
        }

        public static readonly IGremlinQueryExecutor Invalid = Create(static (_, _) => throw new InvalidOperationException($"'{nameof(IGremlinQueryExecutor.Execute)}' must not be called on {nameof(GremlinQueryExecutor)}.{nameof(Invalid)}. If you are getting this exception while executing a query, set a proper {nameof(GremlinQueryExecutor)} on the {nameof(GremlinQuerySource)} (e.g. with 'g.UseGremlinServer(...)' for GremlinServer which can be found in the 'ExRam.Gremlinq.Providers.GremlinServer' package)."));

        public static readonly IGremlinQueryExecutor Identity = Create(static (query, _) => new object[] { query }.ToAsyncEnumerable());

        public static readonly IGremlinQueryExecutor Empty = Create(static (_, _) => AsyncEnumerable.Empty<object>());

        public static IGremlinQueryExecutor Create(Func<IGremlinQueryBase, IGremlinQueryEnvironment, IAsyncEnumerable<object>> executor) => new GremlinQueryExecutorImpl(executor);

        public static IGremlinQueryExecutor TransformQuery(this IGremlinQueryExecutor baseExecutor, Func<IGremlinQueryBase, IGremlinQueryBase> transformation) => new TransformQueryGremlinQueryExecutor(baseExecutor, transformation);

        public static IGremlinQueryExecutor TransformResult(this IGremlinQueryExecutor baseExecutor, Func<IAsyncEnumerable<object>, IAsyncEnumerable<object>> transformation) => new TransformResultGremlinQueryExecutor(baseExecutor, transformation);
    }
}
