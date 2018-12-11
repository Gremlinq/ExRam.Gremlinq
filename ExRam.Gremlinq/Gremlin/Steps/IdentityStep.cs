namespace ExRam.Gremlinq
{
    public sealed class IdentityStep : Step
    {
        public static readonly IdentityStep Instance = new IdentityStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
