using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public abstract class HasStepBase : NonTerminalStep
    {
        private readonly string _name;
        private readonly Expression _expression;

        protected HasStepBase(string name, Expression expression, Option<object> value = default)
        {
            Value = value;
            _name = name;
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
                .Bind<MethodStep>(v =>
                {
                    if (v == P.False)
                        return MethodStep.Create(_name, key, GremlinQuery.Anonymous.Not(_ => _.Identity()).Resolve(model));

                    if (v == P.True)
                        return default;

                    if (v is P.Eq eq)
                        return MethodStep.Create(_name, key, eq.Argument);

                    return MethodStep.Create(_name, key, v);
                })
                .IfNone(() => MethodStep.Create(_name, key));
        }

        internal Option<object> Value { get; }
    }
}
