namespace ExRam.Gremlinq.Core.Execution
{
    public readonly struct GremlinQueryExecutionContext
    {
        private readonly Guid? _executionId;
        private readonly IGremlinQueryBase? _query;

        public GremlinQueryExecutionContext(IGremlinQueryBase query) : this(query, Guid.NewGuid())
        {

        }

        public GremlinQueryExecutionContext(IGremlinQueryBase query, Guid executionId)
        {
            _query = query;
            _executionId = executionId;
        }

        public Guid ExecutionId => _executionId ?? throw new InvalidOperationException();

        public IGremlinQueryBase Query => _query ?? throw new InvalidOperationException();
    }
}
