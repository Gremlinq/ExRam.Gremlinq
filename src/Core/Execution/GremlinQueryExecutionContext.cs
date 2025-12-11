using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core.Execution
{
    /// <summary>
    /// Represents the execution context for a Gremlin query, including the query itself and its execution identifier.
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
        /// Creates a new execution context with a new execution identifier, preserving the query.
        /// </summary>
        /// <returns>A new execution context with a new execution identifier.</returns>
        public GremlinQueryExecutionContext WithNewExecutionId() => new(Query, Guid.NewGuid());

        /// <summary>
        /// Creates a new execution context with a transformed query, preserving the execution identifier.
        /// </summary>
        /// <param name="transformation">A function that transforms the query.</param>
        /// <returns>A new execution context with the transformed query.</returns>
        public GremlinQueryExecutionContext TransformQuery(Func<IGremlinQueryBase, IGremlinQueryBase> transformation) => new(transformation(Query), ExecutionId);

        /// <summary>
        /// Gets the unique identifier for this query execution.
        /// </summary>
        public Guid ExecutionId => _executionId ?? throw UninitializedStruct();

        /// <summary>
        /// Gets the query being executed.
        /// </summary>
        public IGremlinQueryBase Query => _query ?? throw UninitializedStruct();

        /// <summary>
        /// Creates a new execution context for the specified query with a new execution identifier.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <returns>A new execution context.</returns>
        public static GremlinQueryExecutionContext Create(IGremlinQueryBase query) => new(query, Guid.NewGuid());
    }
}
