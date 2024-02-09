using System.Collections;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal readonly struct ExpressionFragment
    {
        private readonly object? _value;

        public static readonly ExpressionFragment True = new(ExpressionFragmentType.Constant, true, default);
        public static readonly ExpressionFragment False = new(ExpressionFragmentType.Constant, false, default);
        public static readonly ExpressionFragment Null = new(ExpressionFragmentType.Constant, default, default);

        private ExpressionFragment(ExpressionFragmentType type, object? value, Expression? expression = default)
        {
            Type = type;
            _value = value;
            Expression = expression;
        }

        public object? TryGetValue() => Type is ExpressionFragmentType.Constant or ExpressionFragmentType.StepLabel ? _value : throw new InvalidOperationException();

        public bool Equals(ExpressionFragment other) => Equals(_value, other._value) && Equals(Expression, other.Expression) && Type == other.Type;

        public override bool Equals(object? obj) => obj is ExpressionFragment other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _value != null ? _value.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Expression != null ? Expression.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Type;
                return hashCode;
            }
        }

        public Expression? Expression { get; }

        public ExpressionFragmentType Type { get; }

        public static ExpressionFragment Create(object source, IGremlinQueryEnvironment environment)
        {
            if (source is Expression expression)
            {
                expression = expression.StripConvert();

                if (expression.TryGetReferredParameter() is not null)
                    return new(ExpressionFragmentType.Parameter, default, expression.StripConvert());

                if (expression.TryParseStepLabelExpression(out var stepLabel, out var stepLabelExpression))
                    return new(ExpressionFragmentType.StepLabel, stepLabel!, stepLabelExpression);

                return new(
                    ExpressionFragmentType.Constant,
                    expression.GetValue() switch
                    {
                        IEnumerable enumerable when enumerable is not ICollection && !environment.SupportsType(enumerable.GetType()) => enumerable.Cast<object>().ToArray(),
                        { } val => val,
                        _ => null
                    });
            }

            return new(ExpressionFragmentType.Constant, source);
        }
    }
}
