namespace ExRam.Gremlinq
{
    public sealed class OtherVStep : Step
    {
        public static readonly OtherVStep Instance = new OtherVStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
