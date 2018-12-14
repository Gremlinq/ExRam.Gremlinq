namespace ExRam.Gremlinq
{
    internal enum GraphElementType
    {
        None,
        Vertex,
        Edge,
        VertexProperty
    }

    internal static class GraphModelExtensions
    {
        public static object GetIdentifier(this IGraphModel model, GraphElementType elementType, string name)
        {
            return (elementType == GraphElementType.Vertex && name == model.VertexIdPropertyName)
                || (elementType == GraphElementType.Edge && name == model.EdgeIdPropertyName)
                ? (object)T.Id
                : name;
        }
    }
}
