namespace ExRam.Gremlinq
{
    public sealed class CountStep : Step
    {
        public static readonly CountStep Instance = new CountStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
