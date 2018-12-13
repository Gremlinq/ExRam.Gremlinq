using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class UnfoldStep : Step
    {
        public static readonly UnfoldStep Instance = new UnfoldStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
