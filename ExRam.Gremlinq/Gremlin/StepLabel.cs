using System;

namespace ExRam.Gremlinq
{
    public abstract class StepLabel : IGremlinSerializable
    {
        public abstract void Serialize(IMethodStringBuilder builder, IParameterCache parameterCache);
    }

    public class StepLabel<TElement> : StepLabel
    {
        public override void Serialize(IMethodStringBuilder builder, IParameterCache parameterCache)
        {
            builder.AppendConstant(this, parameterCache);
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
