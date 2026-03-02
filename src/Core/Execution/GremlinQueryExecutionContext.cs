using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core.Execution
{
    /// <summary>
    /// Represents the context for a Gremlin query execution, including the query and a unique execution identifier.
    /// </summary>
    public readonly struct GremlinQueryExecutionContext
    {
        private readonly Guid? _executionId;
        private readonly IGremlinQueryBase? _query;

        private GremlinQueryExecutionContext(IGremlinQueryBase query, Guid executionId)
        {
            _query = query;
            _executionId = executionId;
        }

        /// <summary>
        /// Returns a new context with a fresh execution identifier.
        /// </summary>
        public GremlinQueryExecutionContext WithNewExecutionId() => new(Query, Guid.NewGuid());

        /// <summary>
        /// Returns a new context with the query transformed by the specified function.
        /// </summary>
        /// <param name="transformation">A function that transforms the query.</param>
        public GremlinQueryExecutionContext TransformQuery(Func<IGremlinQueryBase, IGremlinQueryBase> transformation)
        {
            ArgumentNullException.ThrowIfNull(transformation);

            return new(transformation(Query), ExecutionId);
        }

        /// <summary>
        /// Gets the unique execution identifier.
        /// </summary>
        public Guid ExecutionId => _executionId ?? throw UninitializedStruct();

        /// <summary>
        /// Gets the query associated with this execution context.
        /// </summary>
        public IGremlinQueryBase Query => _query ?? throw UninitializedStruct();

        /// <summary>
        /// Creates a new execution context for the specified query.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        public static GremlinQueryExecutionContext Create(IGremlinQueryBase query)
        {
            ArgumentNullException.ThrowIfNull(query);

            return new(query, Guid.NewGuid());
        }
    }
}
