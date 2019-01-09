using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public sealed class ExpressionNotSupportedException : NotSupportedException
    {
        private const string StandardMessage = "An expression is not supported.";

        public ExpressionNotSupportedException(Expression expression) : base($"The expression '{expression}' is not supported.")
        {

        }

        public ExpressionNotSupportedException(Expression expression, Exception innerException) : base($"The expression '{expression}' is not supported.", Unwrap(innerException))
        {

        }

        public ExpressionNotSupportedException(Exception innerException) : base(StandardMessage, Unwrap(innerException))
        {

        }

        public ExpressionNotSupportedException(string message) : base(message)
        {

        }

        public ExpressionNotSupportedException() : base(StandardMessage)
        {

        }

        private static Exception Unwrap(Exception ex)
        {
            return ex is ExpressionNotSupportedException exprEx && exprEx.Message == StandardMessage
                ? exprEx.InnerException
                : ex;
        }
    }
}
