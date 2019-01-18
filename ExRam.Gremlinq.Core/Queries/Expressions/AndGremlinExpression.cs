namespace System.Linq.Expressions
{
    internal sealed class AndGremlinExpression : BinaryGremlinExpression
    {
        public AndGremlinExpression(Expression parameter, GremlinExpression operand1, GremlinExpression operand2) : base(parameter, operand1, operand2)
        {
        }

        public override GremlinExpression Negate()
        {
            return new OrGremlinExpression(Parameter, Operand1.Negate(), Operand2.Negate());
        }
    }
}
