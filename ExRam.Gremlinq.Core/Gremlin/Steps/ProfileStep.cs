using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class ProfileStep : Step
    {
        public static readonly ProfileStep Instance = new ProfileStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
