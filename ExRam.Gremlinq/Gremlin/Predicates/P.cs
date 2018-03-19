using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExRam.Gremlinq
{
    internal struct P : IGremlinSerializable
    {
        private readonly string _name;
        private readonly object[] _arguments;

        private static readonly IReadOnlyDictionary<ExpressionType, Func<object, P>> SupportedComparisons = new Dictionary<ExpressionType, Func<object, P>>
        {
            { ExpressionType.Equal, P.Eq },
            { ExpressionType.NotEqual, P.Neq },
            { ExpressionType.LessThan, P.Lt },
            { ExpressionType.LessThanOrEqual, P.Lte },
            { ExpressionType.GreaterThanOrEqual, P.Gte },
            { ExpressionType.GreaterThan, P.Gt }
        };

        private P(string name, params object[] arguments)
        {
            this._name = name;
            this._arguments = arguments;
        }

        public GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            var ret = state
                .AppendIdentifier(stringBuilder, nameof(P));

            return this._arguments.Length == 1
                ? ret.AppendMethod(stringBuilder, this._name, this._arguments[0])
                : ret.AppendMethod(stringBuilder, this._name, this._arguments);
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
            return P.SupportedComparisons[expressionType](argument);
        }

        public static P Within(params object[] arguments)
        {
            return new P("within", arguments);
        }
    }
}