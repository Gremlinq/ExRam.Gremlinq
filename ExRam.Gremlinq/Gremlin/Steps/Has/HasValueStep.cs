namespace ExRam.Gremlinq
{
    public sealed class HasValueStep : Step
    {
        public HasValueStep(object argument)
        {
            Argument = argument;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public object Argument { get; }
    }
}
