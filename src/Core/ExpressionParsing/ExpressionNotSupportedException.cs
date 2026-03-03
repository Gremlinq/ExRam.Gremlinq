using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// The exception that is thrown when a LINQ expression is not supported by the Gremlinq expression parser.
    /// </summary>
    public sealed class ExpressionNotSupportedException : NotSupportedException
    {
        private const string StandardMessage = "An expression is not supported.";

        public ExpressionNotSupportedException(Expression expression) : base($"The expression '{expression}' is not supported.")
        {
            ArgumentNullException.ThrowIfNull(expression);

        }

        public ExpressionNotSupportedException(Expression expression, Exception innerException) : base($"The expression '{expression}' is not supported.", Unwrap(innerException))
        {
            ArgumentNullException.ThrowIfNull(expression);
            ArgumentNullException.ThrowIfNull(innerException);

        }

        public ExpressionNotSupportedException(Exception innerException) : base(StandardMessage, Unwrap(innerException))
        {
            ArgumentNullException.ThrowIfNull(innerException);

        }

        public ExpressionNotSupportedException(string message) : base(message)
        {
            ArgumentNullException.ThrowIfNull(message);

        }

        public ExpressionNotSupportedException() : base(StandardMessage)
        {

        }

        private static Exception Unwrap(Exception ex) => ex is ExpressionNotSupportedException { Message: StandardMessage, InnerException: { } innerException }
            ? innerException
            : ex;
    }
}
