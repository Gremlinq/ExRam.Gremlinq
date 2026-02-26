using static ExRam.Gremlinq.Core.ExceptionHelper;

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

        public GremlinQueryExecutionContext TransformQuery(Func<IGremlinQueryBase, IGremlinQueryBase> transformation)
        {
            ArgumentNullException.ThrowIfNull(transformation);

            return new(transformation(Query), ExecutionId);
        }

        public Guid ExecutionId => _executionId ?? throw UninitializedStruct();

        public IGremlinQueryBase Query => _query ?? throw UninitializedStruct();

        public static GremlinQueryExecutionContext Create(IGremlinQueryBase query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return new(query, Guid.NewGuid());
        }
    }
}
