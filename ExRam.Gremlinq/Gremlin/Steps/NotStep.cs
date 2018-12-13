using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class NotStep : SingleTraversalArgumentStep
    {
        public NotStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
