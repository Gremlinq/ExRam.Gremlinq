using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class InVStep : Step
    {
        public static readonly InVStep Instance = new InVStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
