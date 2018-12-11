namespace ExRam.Gremlinq
{
    public sealed class IdStep : Step
    {
        public static readonly IdStep Instance = new IdStep();
        
        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
