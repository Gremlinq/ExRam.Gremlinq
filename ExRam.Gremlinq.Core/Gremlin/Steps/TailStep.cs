using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class TailStep : Step
    {
        public TailStep(long count)
        {
            Count = count;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public long Count { get; }
    }
}
