using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class MapStep : SingleTraversalArgumentStep
    {
        public MapStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
