using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class CreateStep : Step
    {
        public static readonly CreateStep Instance = new CreateStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
