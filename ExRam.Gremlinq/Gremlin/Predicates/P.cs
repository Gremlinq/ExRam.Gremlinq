using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExRam.Gremlinq
{
    public sealed class P : IGroovySerializable
    {
        private readonly string _name;
        private readonly object[] _arguments;

        private static readonly IReadOnlyDictionary<ExpressionType, Func<object, P>> SupportedComparisons = new Dictionary<ExpressionType, Func<object, P>>
        {
            { ExpressionType.Equal, Eq },
            { ExpressionType.NotEqual, Neq },
            { ExpressionType.LessThan, Lt },
            { ExpressionType.LessThanOrEqual, Lte },
            { ExpressionType.GreaterThanOrEqual, Gte },
            { ExpressionType.GreaterThan, Gt }
        };

        private P(string name, params object[] arguments)
        {
            _name = name;
            _arguments = arguments;
        }

        public GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            var ret = state
                .AppendIdentifier(stringBuilder, nameof(P));

            return _arguments.Length == 1
                ? ret.AppendMethod(stringBuilder, _name, _arguments[0])
                : ret.AppendMethod(stringBuilder, _name, _arguments);
        }

        public static P Between(object lower, object upper)
        {
            return new P("between", lower, upper);
        }

        public static P Eq(object argument)
        {
            return new P("eq", argument);
        }

        public static P Neq(object argument)
        {
            return new P("neq", argument);
        }

        public static P Lt(object argument)
        {
            return new P("lt", argument);
        }

        public static P Lte(object argument)
        {
            return new P("lte", argument);
        }

        public static P Gt(object argument)
        {
            return new P("gt", argument);
        }

        public static P Gte(object argument)
        {
            return new P("gte", argument);
        }

        public static P ForExpressionType(ExpressionType expressionType, object argument)
        {
            return SupportedComparisons[expressionType](argument);
        }

        public static P Within(params object[] arguments)
        {
            return new P("within", arguments);
        }

        internal static readonly P True = new P("true");
        internal static readonly P False = new P("false");
    }
}
