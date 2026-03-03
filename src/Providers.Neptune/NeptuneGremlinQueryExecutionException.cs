using ExRam.Gremlinq.Core.Execution;

namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// A Gremlin query execution exception that carries a Neptune-specific error code.
    /// </summary>
    public sealed class NeptuneGremlinQueryExecutionException : GremlinQueryExecutionException
    {
        /// <summary>
        /// Initializes a new <see cref="NeptuneGremlinQueryExecutionException"/> with the specified error code, execution context, message, and inner exception.
        /// </summary>
        /// <param name="code">The Neptune error code.</param>
        /// <param name="executionContext">The execution context of the failed query.</param>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public NeptuneGremlinQueryExecutionException(NeptuneErrorCode code, GremlinQueryExecutionContext executionContext, string message, Exception innerException) : base(executionContext, message, innerException)
        {
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(innerException);

            Code = code;
        }

        /// <summary>
        /// Initializes a new <see cref="NeptuneGremlinQueryExecutionException"/> with the specified error code, execution context, and inner exception.
        /// </summary>
        /// <param name="code">The Neptune error code.</param>
        /// <param name="executionContext">The execution context of the failed query.</param>
        /// <param name="innerException">The inner exception.</param>
        public NeptuneGremlinQueryExecutionException(NeptuneErrorCode code, GremlinQueryExecutionContext executionContext, Exception innerException) : base(executionContext, innerException)
        {
            ArgumentNullException.ThrowIfNull(innerException);

            Code = code;
        }

        /// <summary>
        /// Gets the Neptune error code associated with this exception.
        /// </summary>
        public NeptuneErrorCode Code { get; }
    }
}
