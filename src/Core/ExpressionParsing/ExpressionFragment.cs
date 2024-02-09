using System.Collections;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal readonly struct ExpressionFragment
    {
        private readonly object? _value;

        public static readonly ExpressionFragment True = new(true, default);
        public static readonly ExpressionFragment False = new(false, default);
        public static readonly ExpressionFragment Null = new(default, default);

        private ExpressionFragment(object? value, Expression? expression = default)
        {
            _value = value;
            Expression = expression;
        }

        public object? TryGetValue() => _value;

        public bool Equals(ExpressionFragment other) => Equals(_value, other._value) && Equals(Expression, other.Expression);

        public override bool Equals(object? obj) => obj is ExpressionFragment other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _value != null ? _value.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Expression != null ? Expression.GetHashCode() : 0);
                return hashCode;
            }
        }

        public Expression? Expression { get; }

        public static ExpressionFragment Create(object source, IGremlinQueryEnvironment environment)
        {
            if (source is Expression expression)
            {
                expression = expression.StripConvert();

                if (expression.RefersToParameter(out _))
                    return new(default, expression.StripConvert());

                if (expression.TryParseStepLabelExpression(out var stepLabel, out var stepLabelExpression))
                    return new(stepLabel!, stepLabelExpression);

                return new(
                    expression.GetValue() switch
                    {
                        IEnumerable enumerable when enumerable is not ICollection && !environment.SupportsType(enumerable.GetType()) => enumerable.Cast<object>().ToArray(),
                        { } val => val,
                        _ => null
                    });
            }

            return new(source);
        }
    }
}
