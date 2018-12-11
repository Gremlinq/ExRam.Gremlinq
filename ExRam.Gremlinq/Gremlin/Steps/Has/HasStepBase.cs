using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public abstract class HasStepBase : Step
    {
        public Expression Expression { get; }

        protected HasStepBase(Expression expression, Option<object> value = default)
        {
            Value = value
                .IfNone(P.True);

            Expression = expression;
        }

        internal object Value { get; }
    }
}
