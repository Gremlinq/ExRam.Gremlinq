using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class WhereTraversalStep : SingleTraversalArgumentStep
    {
        public WhereTraversalStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
