using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class ExplainStep : Step
    {
        public static readonly ExplainStep Instance = new ExplainStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
