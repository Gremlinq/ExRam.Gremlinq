using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class DropStep : Step
    {
        public static readonly DropStep Instance = new DropStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
