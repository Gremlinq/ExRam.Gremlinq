using System;

namespace ExRam.Gremlinq
{
    public abstract class StepLabel : IGremlinSerializable
    {
        public abstract MethodStringBuilder Serialize(MethodStringBuilder builder, IParameterCache parameterCache);
    }

    public class StepLabel<TElement> : StepLabel
    {
        public override MethodStringBuilder Serialize(MethodStringBuilder builder, IParameterCache parameterCache)
        {
            return builder.AppendConstant(this, parameterCache);
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
