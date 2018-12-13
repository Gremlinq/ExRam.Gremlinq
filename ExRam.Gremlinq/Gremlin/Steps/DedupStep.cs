using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class DedupStep : Step
    {
        public static readonly DedupStep Instance = new DedupStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
