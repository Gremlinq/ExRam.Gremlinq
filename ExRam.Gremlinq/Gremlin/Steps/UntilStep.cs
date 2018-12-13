using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class UntilStep : SingleTraversalArgumentStep
    {
        public UntilStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
