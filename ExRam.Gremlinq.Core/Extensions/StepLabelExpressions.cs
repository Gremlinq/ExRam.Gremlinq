using System;

namespace ExRam.Gremlinq.Core
{
    public static class StepLabelExpressions
    {
        public static bool Contains<TElement>(this StepLabel<TElement[]> stepLabel, TElement element)
        {
            throw new InvalidOperationException($"{nameof(StepLabelExpressions)}.{nameof(Contains)} is not intended to be executed. It's use is only valid within expressions.");
        }
    }
}
