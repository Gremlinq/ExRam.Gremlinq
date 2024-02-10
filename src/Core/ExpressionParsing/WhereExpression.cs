using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal readonly struct WhereExpression : IEquatable<WhereExpression>
    {
        public static readonly WhereExpression True = new(Expressions.True, EqualsExpressionSemantics.Instance, Expressions.True);
        public static readonly WhereExpression False = new(Expressions.True, EqualsExpressionSemantics.Instance, Expressions.False);

        public WhereExpression(Expression left, ExpressionSemantics semantics, Expression right)
        {
            Left = left.StripConvert();
            Right = right.StripConvert();
            Semantics = semantics;
        }

        public bool Equals(WhereExpression other) => Left.Equals(other.Left) && Right.Equals(other.Right) && Semantics == other.Semantics;

        public override bool Equals(object? obj) => obj is WhereExpression other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Left.GetHashCode();
                hashCode = (hashCode * 397) ^ Right.GetHashCode();
                hashCode = (hashCode * 397) ^ Semantics.GetHashCode();
                return hashCode;
            }
        }

        public Expression Left { get; }
        public Expression Right { get; }
        public ExpressionSemantics Semantics { get; }
    }
}
