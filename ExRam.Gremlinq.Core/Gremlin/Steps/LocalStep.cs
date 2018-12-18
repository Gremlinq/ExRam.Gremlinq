using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class LocalStep : SingleTraversalArgumentStep
    {
        public LocalStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
