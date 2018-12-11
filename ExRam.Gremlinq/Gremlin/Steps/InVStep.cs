namespace ExRam.Gremlinq
{
    public sealed class InVStep : Step
    {
        public static readonly InVStep Instance = new InVStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
