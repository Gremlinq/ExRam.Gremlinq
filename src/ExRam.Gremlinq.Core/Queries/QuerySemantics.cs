namespace ExRam.Gremlinq.Core
{
    public enum QuerySemantics
    {
        None = 0,
        Value = 1,
        Element = 2,
        EdgeOrVertex = 3,
        Vertex = 4,
        Edge = 5,
        Property = 6,
        VertexProperty = 7
    }
}
