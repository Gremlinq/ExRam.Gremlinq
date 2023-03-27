namespace ExRam.Gremlinq.Core.Execution
{
    public static class GremlinQueryExecutor
    {
        private sealed class InvalidGremlinQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment) => throw new InvalidOperationException($"'{nameof(IGremlinQueryExecutor.Execute)}' must not be called on {nameof(GremlinQueryExecutor)}.{nameof(Invalid)}. If you are getting this exception while executing a query, set a proper {nameof(GremlinQueryExecutor)} on the {nameof(GremlinQuerySource)} (e.g. with 'g.UseGremlinServer(...)' for GremlinServer which can be found in the 'ExRam.Gremlinq.Providers.GremlinServer' package).");
        }

        private sealed class EmptyGremlinQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment) => AsyncEnumerable.Empty<object>();
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

        public static readonly IGremlinQueryExecutor Empty = new EmptyGremlinQueryExecutor();

        public static readonly IGremlinQueryExecutor Invalid = new InvalidGremlinQueryExecutor();

        public static IGremlinQueryExecutor TransformQuery(this IGremlinQueryExecutor baseExecutor, Func<IGremlinQueryBase, IGremlinQueryBase> transformation) => new TransformQueryGremlinQueryExecutor(baseExecutor, transformation);
    }
}
