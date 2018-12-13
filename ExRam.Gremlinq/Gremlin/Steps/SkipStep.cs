using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class SkipStep : Step
    {
        public SkipStep(int count)
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
