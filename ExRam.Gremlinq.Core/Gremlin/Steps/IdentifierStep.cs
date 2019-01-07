using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class IdentifierStep : Step
    {
        // ReSharper disable once InconsistentNaming
        public static readonly IdentifierStep g = new IdentifierStep("g");
        public static readonly IdentifierStep __ = new IdentifierStep("__");

        public IdentifierStep(string identifier)
        {
            Identifier = identifier;
        }

        public static IdentifierStep Create(string name)
        {
            return name == "g"
                ? g
                : name == "__"
                    ? __
                    : new IdentifierStep(name);
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Identifier { get; }
    }
}
