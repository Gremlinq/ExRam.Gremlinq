using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class TailStep : Step
    {
        public TailStep(int count)
        {
            Count = count;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public int Count { get; }
    }
}
