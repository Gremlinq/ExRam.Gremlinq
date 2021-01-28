using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal enum ExpressionFragmentType
    {
        Constant,
        Parameter,
    }

    internal sealed class ExpressionFragment
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

        public Expression? Expression { get; }

        public ExpressionFragmentType Type { get; }

        public static ExpressionFragment Create(Expression expression, IGraphModel model)
        {
            return expression.RefersToParameter()
                ? Parameter(expression)
                : expression.TryParseStepLabelExpression(out var stepLabel, out var stepLabelExpression)
                    ? StepLabel(stepLabel!, stepLabelExpression)
                    : Constant(expression.GetValue() switch
                    {
                        IEnumerable enumerable when !(enumerable is ICollection) && !model.NativeTypes.Contains(enumerable.GetType()) => enumerable.Cast<object>().ToArray(),
                        { } val => val,
                        _ => null
                    });
        }

        public static ExpressionFragment Constant(object? value) => new(ExpressionFragmentType.Constant, value);

        public static ExpressionFragment StepLabel(StepLabel value, MemberExpression? expression) => new(ExpressionFragmentType.Constant, value, expression);

        public static ExpressionFragment Parameter(Expression expression) => new(ExpressionFragmentType.Parameter, default, expression);

    }
}
