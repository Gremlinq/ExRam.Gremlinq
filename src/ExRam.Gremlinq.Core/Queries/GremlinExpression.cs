using System;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct GremlinExpression : IEquatable<GremlinExpression>
    {
        public static readonly GremlinExpression True = new(ExpressionFragment.True, ExpressionSemantics.Equals, ExpressionFragment.True);
        public static readonly GremlinExpression False = new(ExpressionFragment.True, ExpressionSemantics.Equals, ExpressionFragment.False);

        public GremlinExpression(ExpressionFragment left, ExpressionSemantics semantics, ExpressionFragment right)
        {
            if (left.Type != ExpressionFragmentType.Parameter && right.Type == ExpressionFragmentType.Parameter)
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
                hashCode = (hashCode * 397) ^ (int) Semantics;
                return hashCode;
            }
        }

        public ExpressionFragment Left { get; }
        public ExpressionFragment Right { get; }
        public ExpressionSemantics Semantics { get; }
    }
}
