using System;

namespace ExRam.Gremlinq.Core
{
    public abstract class StepLabel
    {
        
    }

#pragma warning disable 660,661
    public class StepLabel<TElement> : StepLabel
#pragma warning restore 660,661
    {
        internal QuerySemantics QuerySemantics { get; }

        public StepLabel() : this(QuerySemantics.None)
        {

        }

        internal StepLabel(QuerySemantics querySemantics)
        {
            QuerySemantics = querySemantics;
        }

        public static bool operator ==(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator !=(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator >(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator <(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator >=(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator <=(TElement a, StepLabel<TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }
    }

#pragma warning disable 660, 661
    public class StepLabel<TQuery, TElement> : StepLabel<TElement> where TQuery : IGremlinQuery
#pragma warning restore 660,661
    {
        public StepLabel() : base(typeof(TQuery).GetQuerySemantics())
        {

        }

        internal StepLabel(QuerySemantics querySemantics) : base(querySemantics)
        {

        }

        public static bool operator ==(TElement a, StepLabel<TQuery, TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }

        public static bool operator !=(TElement a, StepLabel<TQuery, TElement> b)
        {
            throw new NotImplementedException("Only for expressions.");
        }
    }
}
