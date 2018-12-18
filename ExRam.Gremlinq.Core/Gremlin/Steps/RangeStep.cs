using System;
using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class RangeStep : Step
    {
        public RangeStep(long lower, long upper)
        {
            if (lower < 0)
                throw new ArgumentOutOfRangeException(nameof(lower));

            if (upper < 0)
                throw new ArgumentException(nameof(upper));

            Lower = lower;
            Upper = upper;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public long Lower { get; }
        public long Upper { get; }
    }
}
