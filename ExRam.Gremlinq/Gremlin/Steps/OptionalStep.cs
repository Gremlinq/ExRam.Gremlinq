using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class OptionalStep : SingleTraversalArgumentStep
    {
        public OptionalStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
