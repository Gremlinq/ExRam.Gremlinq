namespace ExRam.Gremlinq
{
    public sealed class SumStep : Step
    {
        public static readonly SumStep Local = new SumStep(Scope.Local);
        public static readonly SumStep Global = new SumStep(Scope.Global);

        public SumStep(Scope scope)
        {
            Scope = scope;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Scope Scope { get; }
    }
}
