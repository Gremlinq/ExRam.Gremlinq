using System;
using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class StepLabel : IGroovySerializable
    {
        public abstract GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state);
    }

#pragma warning disable 660,661
    public class StepLabel<TElement> : StepLabel
#pragma warning restore 660,661
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

    public class VStepLabel<TVertex> : StepLabel<TVertex>
    {
       
    }

    public class EStepLabel<TEdge> : StepLabel<TEdge>
    {

    }
}
