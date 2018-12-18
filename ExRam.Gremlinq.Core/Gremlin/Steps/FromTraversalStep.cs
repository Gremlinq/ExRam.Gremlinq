using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class FromTraversalStep : SingleTraversalArgumentStep
    {
        public FromTraversalStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
