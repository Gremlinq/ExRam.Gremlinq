using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class SideEffectStep : SingleTraversalArgumentStep
    {
        public SideEffectStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
