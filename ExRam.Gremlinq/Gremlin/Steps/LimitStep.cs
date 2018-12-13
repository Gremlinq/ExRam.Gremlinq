using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class LimitStep : Step
    {
        public static readonly LimitStep Limit1 = new LimitStep(1);

        public LimitStep(int limit)
        {
            Limit = limit;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public int Limit { get; }
    }
}
