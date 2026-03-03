using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// The exception that is thrown when a LINQ expression is not supported by the Gremlinq expression parser.
    /// </summary>
    public sealed class ExpressionNotSupportedException : NotSupportedException
    {
        private const string StandardMessage = "An expression is not supported.";

        /// <summary>Initializes a new instance with the specified expression.</summary>
        /// <param name="expression">The unsupported expression.</param>
        public ExpressionNotSupportedException(Expression expression) : base($"The expression '{expression}' is not supported.")
        {
            ArgumentNullException.ThrowIfNull(expression);

        }

        /// <summary>Initializes a new instance with the specified expression and inner exception.</summary>
        /// <param name="expression">The unsupported expression.</param>
        /// <param name="innerException">The inner exception.</param>
        public ExpressionNotSupportedException(Expression expression, Exception innerException) : base($"The expression '{expression}' is not supported.", Unwrap(innerException))
        {
            ArgumentNullException.ThrowIfNull(expression);
            ArgumentNullException.ThrowIfNull(innerException);

        }

        /// <summary>Initializes a new instance with the specified inner exception.</summary>
        /// <param name="innerException">The inner exception.</param>
        public ExpressionNotSupportedException(Exception innerException) : base(StandardMessage, Unwrap(innerException))
        {
            ArgumentNullException.ThrowIfNull(innerException);

        }

        /// <summary>Initializes a new instance with the specified message.</summary>
        /// <param name="message">The error message.</param>
        public ExpressionNotSupportedException(string message) : base(message)
        {
            ArgumentNullException.ThrowIfNull(message);

        }

        /// <summary>Initializes a new instance with a default message.</summary>
        public ExpressionNotSupportedException() : base(StandardMessage)
        {

        }

        private static Exception Unwrap(Exception ex) => ex is ExpressionNotSupportedException { Message: StandardMessage, InnerException: { } innerException }
            ? innerException
            : ex;
    }
}
