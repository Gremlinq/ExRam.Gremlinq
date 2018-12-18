using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class EdgesStep : SingleTraversalArgumentStep
    {
        public EdgesStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
