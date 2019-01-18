namespace System.Linq.Expressions
{
    internal abstract class GremlinExpression
    {
        protected GremlinExpression(Expression parameter)
        {
            Parameter = parameter;
        }

        public abstract GremlinExpression Negate();

        public Expression Parameter { get; }
    }
}
