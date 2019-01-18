namespace System.Linq.Expressions
{
    internal sealed class NotGremlinExpression : GremlinExpression
    {
        public NotGremlinExpression(Expression parameter, GremlinExpression expression) : base(parameter)
        {
            Expression = expression;
        }

        public override GremlinExpression Negate()
        {
            return Expression;
        }

        public GremlinExpression Expression { get; }
    }
}