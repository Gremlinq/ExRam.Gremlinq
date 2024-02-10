using System.Collections;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal static class Expressions
    {
        public static readonly ExpressionFragment True = ExpressionFragment.Create(Expression.Constant(true));
        public static readonly ExpressionFragment False = ExpressionFragment.Create(Expression.Constant(false));
        public static readonly ExpressionFragment Null = ExpressionFragment.Create(Expression.Constant(null, typeof(object)));
    }

    internal readonly struct ExpressionFragment
    {
        private ExpressionFragment(Expression expression)
        {
            Expression = expression;
        }

        public bool Equals(ExpressionFragment other) => Equals(Expression, other.Expression);

        public override bool Equals(object? obj) => obj is ExpressionFragment other && Equals(other);

        public override int GetHashCode() => Expression.GetHashCode();

        public Expression Expression { get; }

        public static ExpressionFragment Create(Expression expression)
        {
            return new(expression.StripConvert());
        }
    }
}
