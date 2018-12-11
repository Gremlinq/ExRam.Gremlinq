namespace ExRam.Gremlinq
{
    public abstract class Step : IQueryElement
    {
        public abstract void Accept(IQueryElementVisitor visitor);
    }
}
