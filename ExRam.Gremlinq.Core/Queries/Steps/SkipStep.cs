using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class SkipStep : Step
    {
        public SkipStep(long count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
        }

        public long Count { get; }
    }
}
