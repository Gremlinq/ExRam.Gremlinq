namespace System.Linq.Expressions
{
    internal sealed class OrGremlinExpression : BinaryGremlinExpression
    {
        public OrGremlinExpression(Expression parameter, GremlinExpression operand1, GremlinExpression operand2) : base(parameter, operand1, operand2)
        {
        }

        public override GremlinExpression Negate()
        {
            return new AndGremlinExpression(Parameter, Operand1.Negate(), Operand2.Negate());
        }
    }
}
