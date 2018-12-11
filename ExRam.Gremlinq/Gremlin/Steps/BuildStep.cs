namespace ExRam.Gremlinq
{
    public sealed class BuildStep : Step
    {
        public static readonly BuildStep Instance = new BuildStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
