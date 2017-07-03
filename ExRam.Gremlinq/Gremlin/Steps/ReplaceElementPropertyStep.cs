using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public sealed class ReplaceElementPropertyStep<TSource, TValue> : NonTerminalGremlinStep
    {
        private readonly string _key;
        private readonly TValue _value;
        private readonly AddElementPropertiesStep _baseStep;

        public ReplaceElementPropertyStep(AddElementPropertiesStep baseStep, Expression<Func<TSource, TValue>> memberExpression, TValue value)
        {
            this._value = value;
            this._baseStep = baseStep;

            if ((this._key = (memberExpression.Body as MemberExpression)?.Member.Name) == null)
                throw new ArgumentException();    
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            foreach (var step in this._baseStep.Resolve(model))
            {
                if (step.Name == "property" && step.Parameters.Count > 0 && step.Parameters[0] as string == this._key)
                    yield return new TerminalGremlinStep("property", this._key, this._value);
                else
                    yield return step;
            }
        }
    }
}