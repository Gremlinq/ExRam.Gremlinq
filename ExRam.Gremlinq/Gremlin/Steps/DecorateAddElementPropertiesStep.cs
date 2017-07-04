using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public abstract class DecorateAddElementPropertiesStep<TSource, TValue> : NonTerminalGremlinStep
    {
        protected DecorateAddElementPropertiesStep(AddElementPropertiesStep baseStep, Expression<Func<TSource, TValue>> memberExpression, TValue value)
        {
            this.Value = value;
            this.BaseStep = baseStep;

            if ((this.Key = (memberExpression.Body as MemberExpression)?.Member.Name) == null)
                throw new ArgumentException();
        }

        protected string Key { get; }
        protected TValue Value { get; }
        protected AddElementPropertiesStep BaseStep { get; }
    }
}