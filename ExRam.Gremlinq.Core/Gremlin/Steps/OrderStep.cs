using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class OrderStep : Step
    {
        public static readonly OrderStep Instance = new OrderStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
