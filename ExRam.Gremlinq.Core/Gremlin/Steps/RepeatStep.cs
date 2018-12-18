using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class RepeatStep : SingleTraversalArgumentStep
    {
        public RepeatStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
