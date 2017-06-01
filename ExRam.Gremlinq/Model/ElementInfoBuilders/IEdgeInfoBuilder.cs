namespace ExRam.Gremlinq
{
    public interface IEdgeInfoBuilder<T>
    {
        IEdgeInfoBuilder<T> Label(string label);
    }
}