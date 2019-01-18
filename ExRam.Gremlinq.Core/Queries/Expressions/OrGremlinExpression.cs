using ExRam.Gremlinq.Core;

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

        protected override P Fuse(P operand1, P operand2)
        {
            return operand1.Or(operand2);
        }

        protected override BinaryGremlinExpression CreateSimplified(Expression parameter, GremlinExpression operand1, GremlinExpression operand2)
        {
            return new OrGremlinExpression(parameter, operand1, operand2);
        }
    }
}
