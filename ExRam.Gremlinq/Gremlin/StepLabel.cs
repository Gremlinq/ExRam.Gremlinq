using System;
using System.Text;

namespace ExRam.Gremlinq
{
    public abstract class StepLabel : IGremlinSerializable
    {
        public abstract void Serialize(StringBuilder builder, IParameterCache parameterCache);
    }

    public class StepLabel<TElement> : StepLabel
    {
        public override void Serialize(StringBuilder builder, IParameterCache parameterCache)
        {
            builder.Append(parameterCache.Cache(this));
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
