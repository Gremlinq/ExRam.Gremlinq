namespace ExRam.Gremlinq
{
    public interface IEdgeInfoBuilder<T>
    {
        EdgeInfo Build();
        IEdgeInfoBuilder<T> Label(string label);
    }
}