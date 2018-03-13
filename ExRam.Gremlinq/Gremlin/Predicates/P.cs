using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExRam.Gremlinq
{
    internal struct P : IGremlinSerializable
    {
        private readonly object _name;
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

        public P(object name, params object[] arguments)
        {
            this._name = name;
            this._arguments = arguments;
        }

        public void Serialize(StringBuilder builder, IParameterCache parameterCache)
        {
            builder.Append(nameof(P));
            builder.Append(".");
            builder.Append(this._name);
            builder.Append("(");

            for (var i = 0; i < this._arguments.Length; i++)
            {
                var parameter = this._arguments[i];

                if (i != 0)
                    builder.Append(", ");

                if (parameter is IGremlinSerializable serializable)
                    serializable.Serialize(builder, parameterCache);
                else
                    builder.Append(parameterCache.Cache(parameter));
            }

            builder.Append(")");
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