using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class FoldStep : Step
    {
        public static readonly FoldStep Instance = new FoldStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
