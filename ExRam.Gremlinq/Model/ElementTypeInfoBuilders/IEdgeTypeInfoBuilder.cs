namespace ExRam.Gremlinq
{
    public interface IEdgeTypeInfoBuilder<T>
    {
        EdgeTypeInfo Build();
        IEdgeTypeInfoBuilder<T> Label(string label);
    }
}