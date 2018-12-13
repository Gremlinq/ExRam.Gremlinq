using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class ToTraversalStep : SingleTraversalArgumentStep
    {
        public ToTraversalStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
