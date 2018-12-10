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
            Value = value
                .IfNone(P.True);

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

            if (Value == P.False)
                yield return new NotStep(GremlinQuery.Anonymous.Resolve(model));
            else if (Value == P.True)
                yield return MethodStep.Create(_name, key);
            else
            {
                yield return MethodStep.Create(
                    _name,
                    key,
                    Value is P.Eq eq
                        ? eq.Argument
                        : Value);
            }
        }

        internal object Value { get; }
    }
}
