using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class VerticesStep : SingleTraversalArgumentStep
    {
        public VerticesStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
