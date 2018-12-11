namespace ExRam.Gremlinq
{
    public sealed class OrderStep : Step
    {
        public static readonly OrderStep Instance = new OrderStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
