using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class OrStep : LogicalStep
    {
        public OrStep(IGremlinQuery[] traversals) : base("or", traversals)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
