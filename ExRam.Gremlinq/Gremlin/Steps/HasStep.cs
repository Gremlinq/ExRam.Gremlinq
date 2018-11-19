using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class HasStep : NonTerminalStep
    {
        private const string Name = "has";

        private readonly Expression _expression;

        public HasStep(Expression expression, Option<object> value = default)
        {
            Value = value;
            _expression = expression;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            string name;

            switch (_expression)
            {
                case MemberExpression leftMemberExpression:
                {
                    name = leftMemberExpression.Member.Name;
                    break;
                }
                case ParameterExpression leftParameterExpression:
                {
                    name = leftParameterExpression.Name;
                    break;
                }
                default:
                    throw new NotSupportedException();
            }

            var key = model.GetIdentifier(name);

            yield return Value
                .Bind<object>(v =>
                {
                    if (v == P.False)
                        return (object)GremlinQuery.Anonymous.Not(_ => _.Identity());

                    if (v == P.True)
                        return default;

                    return v;
                })
                .Match(
                    value => new MethodStep(Name, key, value),
                    () => new MethodStep(Name, key));
        }

        internal Option<object> Value { get; }
    }
}
