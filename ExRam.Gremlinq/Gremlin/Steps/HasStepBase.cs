using System;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public abstract class HasStepBase : Step
    {
        protected HasStepBase(IGraphModel model, Expression expression, Option<object> value = default)
        {
            Value = value
                .IfNone(P.True);

            string memberName;

            switch (expression)
            {
                case MemberExpression leftMemberExpression:
                {
                    memberName = leftMemberExpression.Member.Name;
                    break;
                }
                case ParameterExpression leftParameterExpression:
                {
                    memberName = leftParameterExpression.Name;
                    break;
                }
                default:
                    throw new NotSupportedException();
            }

            Key = model.GetIdentifier(memberName);
        }

        public object Key { get; }
        public object Value { get; }
    }
}
