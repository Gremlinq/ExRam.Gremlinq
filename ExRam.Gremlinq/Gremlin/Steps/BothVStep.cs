namespace ExRam.Gremlinq
{
    public sealed class BothVStep : Step
    {
        public static readonly BothVStep Instance = new BothVStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
