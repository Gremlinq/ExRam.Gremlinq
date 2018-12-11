namespace ExRam.Gremlinq
{
    public sealed class OutVStep : Step
    {
        public static readonly OutVStep Instance = new OutVStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
