using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal readonly struct GremlinExpression : IEquatable<GremlinExpression>
    {
        public static readonly GremlinExpression True = new(Expressions.True, EqualsExpressionSemantics.Instance, Expressions.True);
        public static readonly GremlinExpression False = new(Expressions.True, EqualsExpressionSemantics.Instance, Expressions.False);

        public GremlinExpression(Expression left, ExpressionSemantics semantics, Expression right)
        {
            if (left.RefersToParameter(out _) is not true && right.RefersToParameter(out _) is true)
            {
                Left = right;
                Semantics = semantics.Flip();
                Right = left;
            }
            else
            {
                Left = left;
                Right = right;
                Semantics = semantics;
            }
        }

        public bool Equals(GremlinExpression other) => Left.Equals(other.Left) && Right.Equals(other.Right) && Semantics == other.Semantics;

        public override bool Equals(object? obj) => obj is GremlinExpression other && Equals(other);

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
