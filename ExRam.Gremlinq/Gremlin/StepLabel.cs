using System;
using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class StepLabel : IGremlinSerializable
    {
        public abstract GroovyExpressionBuilder Serialize(StringBuilder stringBuilder, GroovyExpressionBuilder builder);
    }

    public class StepLabel<TElement> : StepLabel
    {
        public override GroovyExpressionBuilder Serialize(StringBuilder stringBuilder, GroovyExpressionBuilder builder)
        {
            return builder.AppendConstant(stringBuilder, this);
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
