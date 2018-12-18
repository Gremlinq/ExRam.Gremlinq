using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class CountStep : Step
    {
        public static readonly CountStep Instance = new CountStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
