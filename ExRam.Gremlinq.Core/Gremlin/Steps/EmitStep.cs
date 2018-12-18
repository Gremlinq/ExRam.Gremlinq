using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class EmitStep : Step
    {
        public static readonly EmitStep Instance = new EmitStep();

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
