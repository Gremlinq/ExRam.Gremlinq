using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class OtherVStep : Step
    {
        public static readonly OtherVStep Instance = new OtherVStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
