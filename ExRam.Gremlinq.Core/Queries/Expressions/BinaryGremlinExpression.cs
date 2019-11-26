using ExRam.Gremlinq.Core;
using Gremlin.Net.Process.Traversal;

namespace System.Linq.Expressions
{
    internal abstract class BinaryGremlinExpression : GremlinExpression
    {
        protected BinaryGremlinExpression(Expression parameter, GremlinExpression operand1, GremlinExpression operand2) : base(parameter)
        {
            Operand1 = operand1;
            Operand2 = operand2;
        }

        public override GremlinExpression Simplify()
        {
            var simplifiedOperand1 = Operand1.Simplify();
            var simplifiedOperand2 = Operand2.Simplify();

            if (simplifiedOperand1 is TerminalGremlinExpression terminal1 && simplifiedOperand2 is TerminalGremlinExpression terminal2)
            {
                if (terminal1.Parameter == terminal2.Parameter)
                {
                    if (terminal1.Key == terminal2.Key || terminal1.Key is MemberExpression memberExpression1 && terminal2.Key is MemberExpression memberExpression2 && memberExpression1.Member == memberExpression2.Member)
                    {
                        return new TerminalGremlinExpression(Parameter, terminal1.Key, Fuse(terminal1.Predicate, terminal2.Predicate));
                    }
                }
            }

            if (simplifiedOperand1 == Operand1 && simplifiedOperand2 == Operand2)
                return this;

            return CreateSimplified(Parameter, simplifiedOperand1, simplifiedOperand2);
        }

        protected abstract P Fuse(P operand1, P operand2);

        protected abstract BinaryGremlinExpression CreateSimplified(Expression parameter, GremlinExpression operand1, GremlinExpression operand2);

        public GremlinExpression Operand1 { get; }
        public GremlinExpression Operand2 { get; }
    }
}
