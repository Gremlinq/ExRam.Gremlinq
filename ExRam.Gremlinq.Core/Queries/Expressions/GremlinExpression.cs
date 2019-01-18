namespace System.Linq.Expressions
{
    internal abstract class GremlinExpression
    {
        protected GremlinExpression(Expression parameter)
        {
            Parameter = parameter;
        }

        public abstract GremlinExpression Negate();
        public abstract GremlinExpression Simplify();

        public Expression Parameter { get; }
    }
}
