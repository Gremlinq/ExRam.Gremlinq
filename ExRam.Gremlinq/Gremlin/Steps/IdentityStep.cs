using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class IdentityStep : Step
    {
        public static readonly IdentityStep Instance = new IdentityStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
