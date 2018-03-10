using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExRam.Gremlinq
{
    internal struct GremlinPredicate : IGremlinSerializable
    {
        private readonly object _name;
        private readonly object[] _arguments;

        private static readonly IReadOnlyDictionary<ExpressionType, Func<object, GremlinPredicate>> SupportedComparisons = new Dictionary<ExpressionType, Func<object, GremlinPredicate>>
        {
            { ExpressionType.Equal, GremlinPredicate.Eq },
            { ExpressionType.NotEqual, GremlinPredicate.Neq },
            { ExpressionType.LessThan, GremlinPredicate.Lt },
            { ExpressionType.LessThanOrEqual, GremlinPredicate.Lte },
            { ExpressionType.GreaterThanOrEqual, GremlinPredicate.Gte },
            { ExpressionType.GreaterThan, GremlinPredicate.Gt }
        };

        public GremlinPredicate(object name, params object[] arguments)
        {
            this._name = name;
            this._arguments = arguments;
        }

        public void Serialize(StringBuilder builder, IParameterCache parameterCache)
        {
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

        public static GremlinPredicate Eq(object argument)
        {
            return new GremlinPredicate("P.eq", argument);
        }

        public static GremlinPredicate Neq(object argument)
        {
            return new GremlinPredicate("P.neq", argument);
        }

        public static GremlinPredicate Lt(object argument)
        {
            return new GremlinPredicate("P.lt", argument);
        }

        public static GremlinPredicate Lte(object argument)
        {
            return new GremlinPredicate("P.lte", argument);
        }

        public static GremlinPredicate Gt(object argument)
        {
            return new GremlinPredicate("P.gt", argument);
        }

        public static GremlinPredicate Gte(object argument)
        {
            return new GremlinPredicate("P.gte", argument);
        }

        public static GremlinPredicate ForExpressionType(ExpressionType expressionType, object argument)
        {
            return GremlinPredicate.SupportedComparisons[expressionType](argument);
        }

        public static GremlinPredicate Within(params object[] arguments)
        {
            return new GremlinPredicate("P.within", arguments);
        }
    }
}