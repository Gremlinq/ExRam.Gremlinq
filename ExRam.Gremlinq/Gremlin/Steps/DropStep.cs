namespace ExRam.Gremlinq
{
    public sealed class DropStep : Step
    {
        public static readonly DropStep Instance = new DropStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
