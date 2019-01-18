namespace System.Linq.Expressions
{
    internal abstract class BinaryGremlinExpression : GremlinExpression
    {
        protected BinaryGremlinExpression(Expression parameter, GremlinExpression operand1, GremlinExpression operand2) : base(parameter)
        {
            Operand1 = operand1;
            Operand2 = operand2;
        }

        public GremlinExpression Operand1 { get; }
        public GremlinExpression Operand2 { get; }
    }
}
