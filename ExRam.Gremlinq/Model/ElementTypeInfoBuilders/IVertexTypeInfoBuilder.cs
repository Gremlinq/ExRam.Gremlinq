namespace ExRam.Gremlinq
{
    public interface IVertexTypeInfoBuilder<T>
    {
        VertexTypeInfo Build();
        IVertexTypeInfoBuilder<T> Label(string label);
    }
}