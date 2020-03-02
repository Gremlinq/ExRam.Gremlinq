#pragma warning disable 660,661
using System;

namespace ExRam.Gremlinq.Core
{
    public abstract class StepLabel
    {
        
    }

    public class StepLabel<TElement> : StepLabel
    {
        public static implicit operator TElement(StepLabel<TElement> stepLabel)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator ==(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator !=(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public TElement Value
        {
            get
            {
                throw new NotImplementedException("Only for expressions.");
            }
        }
    }

    public class StepLabel<TQuery, TElement> : StepLabel<TElement> where TQuery : IGremlinQueryBase
    {
        
    }
}
#pragma warning restore 660,661
