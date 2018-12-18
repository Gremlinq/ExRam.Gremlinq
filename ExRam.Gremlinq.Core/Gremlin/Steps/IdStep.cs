using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class IdStep : Step
    {
        public static readonly IdStep Instance = new IdStep();
        
        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
