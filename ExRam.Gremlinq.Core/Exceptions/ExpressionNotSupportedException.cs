using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public sealed class ExpressionNotSupportedException : NotSupportedException
    {
        public ExpressionNotSupportedException(Expression expression) : base($"The expression '{expression}' is not supported.")
        {

        }

        public ExpressionNotSupportedException(Expression expression, Exception innerException) : base($"The expression '{expression}' is not supported.", innerException)
        {

        }

        public ExpressionNotSupportedException(Exception innerException) : base("An expression is not supported.", innerException)
        {

        }

        public ExpressionNotSupportedException() : base("An expression is not supported.")
        {

        }
    }
}
