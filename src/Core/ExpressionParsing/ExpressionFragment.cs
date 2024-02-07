using System.Collections;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal readonly struct ExpressionFragment
    {
        private readonly object? _value;

        public static readonly ExpressionFragment True = Constant(true, default);
        public static readonly ExpressionFragment False = Constant(false, default);
        public static readonly ExpressionFragment Null = Constant(default, default);

        private ExpressionFragment(ExpressionFragmentType type, object? value, WellKnownMember? wellKnownMember, Expression? expression = default)
        {
            Type = type;
            _value = value;
            Expression = expression;
            WellKnownMember = wellKnownMember;
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
        public WellKnownMember? WellKnownMember { get; }

        public static ExpressionFragment Create(Expression expression, IGremlinQueryEnvironment environment)
        {
            expression = expression.StripConvert();

            return expression.TryGetReferredParameter() is not null
                ? Parameter(expression)
                : expression.TryParseStepLabelExpression(out var stepLabel, out var stepLabelExpression)
                    ? StepLabel(stepLabel!, stepLabelExpression)
                    : Constant(expression.GetValue() switch
                    {
                        IEnumerable enumerable when enumerable is not ICollection && !environment.SupportsType(enumerable.GetType()) => enumerable.Cast<object>().ToArray(),
                        { } val => val,
                        _ => null
                    }, default);
        }

        public static ExpressionFragment Constant(object? value, WellKnownMember? wellKnownMember) => new(ExpressionFragmentType.Constant, value, wellKnownMember);

        public static ExpressionFragment StepLabel(StepLabel value, MemberExpression? expression) => new(ExpressionFragmentType.StepLabel, value, default, expression);

        public static ExpressionFragment Parameter(Expression expression) => new(ExpressionFragmentType.Parameter, default, default, expression.StripConvert());
    }
}
