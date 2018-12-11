namespace ExRam.Gremlinq
{
    public interface IQueryElement
    {
        void Accept(IQueryElementVisitor visitor);
    }
}
