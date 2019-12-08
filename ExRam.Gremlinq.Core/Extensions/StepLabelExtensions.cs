using System;

namespace ExRam.Gremlinq.Core
{
    public static class StepLabelExtensions
    {
        public static bool Contains<TElement>(this StepLabel<TElement[]> stepLabel, TElement element)
        {
            throw new InvalidOperationException($"{nameof(StepLabelExtensions)}.{nameof(Contains)} is not intended to be executed. It's use is only valid within expressions.");
        }
    }
}
