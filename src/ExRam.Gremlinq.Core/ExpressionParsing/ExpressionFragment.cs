using System.Collections;
using System.Linq;
using System.Linq.Expressions;

using ExRam.Gremlinq.Core.Models;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal readonly struct ExpressionFragment
    {
        private readonly object? _value;

        public static readonly ExpressionFragment True = Constant(true);
        public static readonly ExpressionFragment False = Constant(false);
        public static readonly ExpressionFragment Null = Constant(default);

        private ExpressionFragment(ExpressionFragmentType type, object? value, Expression? expression = default)
        {
            Type = type;
            _value = value;
            Expression = expression;
        }

        public object? GetValue() => Type == ExpressionFragmentType.Constant ? _value : Expression?.GetValue();

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

        public static ExpressionFragment Create(Expression expression, IGraphModel model)
        {
            expression = expression.StripConvert();

            return expression.TryGetReferredParameter() is not null
                ? Parameter(expression)
                : expression.TryParseStepLabelExpression(out var stepLabel, out var stepLabelExpression)
                    ? StepLabel(stepLabel!, stepLabelExpression)
                    : Constant(expression.GetValue() switch
                    {
                        IEnumerable enumerable when enumerable is not ICollection && !model.NativeTypes.Contains(enumerable.GetType()) => enumerable.Cast<object>().ToArray(),
                        { } val => val,
                        _ => null
                    });
        }

        public static ExpressionFragment Constant(object? value) => new(ExpressionFragmentType.Constant, value);

        public static ExpressionFragment StepLabel(StepLabel value, MemberExpression? expression) => new(ExpressionFragmentType.Constant, value, expression);

        public static ExpressionFragment Parameter(Expression expression) => new(ExpressionFragmentType.Parameter, default, expression.StripConvert());
    }
}
