using System.Linq.Expressions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
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

        protected override P Fuse(P operand1, P operand2)
        {
            return operand1.And(operand2);
        }

        protected override BinaryGremlinExpression CreateSimplified(Expression parameter, GremlinExpression operand1, GremlinExpression operand2)
        {
            return new AndGremlinExpression(parameter, operand1, operand2);
        }
    }
}
