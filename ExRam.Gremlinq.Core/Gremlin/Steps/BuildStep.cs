using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class BuildStep : Step
    {
        public static readonly BuildStep Instance = new BuildStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
