using ExRam.Gremlinq.Core;

namespace System.Linq.Expressions
{
    internal sealed class TerminalGremlinExpression : GremlinExpression
    {
        public TerminalGremlinExpression(Expression parameter, Expression key, P predicate) : base(parameter)
        {
            Key = key;
            Predicate = predicate;
        }

        public override GremlinExpression Negate()
        {
            return new NotGremlinExpression(Parameter, this);
        }

        public override GremlinExpression Simplify()
        {
            return this;
        }

        public P Predicate { get; }
        public Expression Key { get; }
    }
}
