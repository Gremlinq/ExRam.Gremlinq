using System;
using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class StepLabel : IGremlinSerializable
    {
        public abstract GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state);
    }

    public class StepLabel<TElement> : StepLabel
    {
        public override GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            return state.AppendConstant(stringBuilder, this);
        }

        public static bool operator ==(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator !=(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }
    }
}
