namespace ExRam.Gremlinq.Core.Execution
{
    public readonly struct GremlinQueryExecutionContext
    {
        private readonly Guid? _executionId;
        private readonly IGremlinQueryBase? _query;

        private GremlinQueryExecutionContext(IGremlinQueryBase query, Guid executionId)
        {
            _query = query;
            _executionId = executionId;
        }

        public GremlinQueryExecutionContext WithNewExecutionId() => new(Query, Guid.NewGuid());

        public GremlinQueryExecutionContext TransformQuery(Func<IGremlinQueryBase, IGremlinQueryBase> transformation) => new(transformation(Query), ExecutionId);

        public Guid ExecutionId => _executionId ?? throw new InvalidOperationException();

        public IGremlinQueryBase Query => _query ?? throw new InvalidOperationException();

        public static GremlinQueryExecutionContext Create(IGremlinQueryBase query) => new(query, Guid.NewGuid());
    }
}
