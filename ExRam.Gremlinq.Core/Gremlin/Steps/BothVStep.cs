using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class BothVStep : Step
    {
        public static readonly BothVStep Instance = new BothVStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
