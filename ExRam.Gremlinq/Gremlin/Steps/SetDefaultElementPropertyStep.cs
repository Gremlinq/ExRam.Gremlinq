using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public sealed class SetDefaultElementPropertyStep<TSource, TValue> : DecorateAddElementPropertiesStep<TSource, TValue>
    {
        public SetDefaultElementPropertyStep(AddElementPropertiesStep baseStep, Expression<Func<TSource, TValue>> memberExpression, TValue value) : base(baseStep, memberExpression, value)
        {
        }
        
        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            var seen = false;
            foreach (var step in this.BaseStep.Resolve(model))
            {
                seen |= (step.Name == "property" && step.Parameters.Count > 0 && step.Parameters[0] as string == this.Key);
                yield return step;
            }

            if (!seen)
                yield return new TerminalGremlinStep("property", this.Key, this.Value);
        }
    }
}