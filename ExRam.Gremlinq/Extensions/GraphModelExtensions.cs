namespace ExRam.Gremlinq
{
    internal static class GraphModelExtensions
    {
        public static object GetIdentifier(this IGraphModel model, string name)
        {
            return name == model.IdPropertyName
                ? (object)T.Id
                : name;
        }
    }
}