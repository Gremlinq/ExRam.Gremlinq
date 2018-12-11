namespace ExRam.Gremlinq
{
    public sealed class BarrierStep : Step
    {
        public static readonly BarrierStep Instance = new BarrierStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
