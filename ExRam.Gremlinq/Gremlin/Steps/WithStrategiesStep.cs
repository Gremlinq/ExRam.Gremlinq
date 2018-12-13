using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class WithStrategiesStep : SingleTraversalArgumentStep
    {
        public WithStrategiesStep(IGremlinQuery traversal) : base(traversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
