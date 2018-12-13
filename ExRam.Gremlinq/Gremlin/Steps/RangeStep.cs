using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class RangeStep : Step
    {
        public RangeStep(int lower, int upper)
        {
            Lower = lower;
            Upper = upper;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public int Lower { get; }
        public int Upper { get; }
    }
}
