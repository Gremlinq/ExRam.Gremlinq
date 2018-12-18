using System;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public abstract class StepLabel : IGremlinQueryElement
    {
        public void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

#pragma warning disable 660,661
    public class StepLabel<TElement> : StepLabel
#pragma warning restore 660,661
    {
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

    // ReSharper disable once UnusedTypeParameter
    public class EStepLabel<TEdge, TAdjacentVertex> : StepLabel<TEdge>
    {

    }

    // ReSharper disable once UnusedTypeParameter
    public class OutEStepLabel<TEdge, TAdjacentVertex> : StepLabel<TEdge>
    {

    }


    // ReSharper disable once UnusedTypeParameter
    public class InEStepLabel<TEdge, TAdjacentVertex> : StepLabel<TEdge>
    {

    }
}
