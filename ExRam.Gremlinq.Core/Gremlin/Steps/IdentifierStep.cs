using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class IdentifierStep : Step
    {
        public IdentifierStep(string identifier)
        {
            Identifier = identifier;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Identifier { get; }
    }
}
