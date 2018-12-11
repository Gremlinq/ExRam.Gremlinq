namespace ExRam.Gremlinq
{
    public sealed class CreateStep : Step
    {
        public static readonly CreateStep Instance = new CreateStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
