namespace ExRam.Gremlinq
{
    public sealed class IdentifierStep : Step
    {
        public IdentifierStep(string identifier)
        {
            Identifier = identifier;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Identifier { get; }
    }
}
