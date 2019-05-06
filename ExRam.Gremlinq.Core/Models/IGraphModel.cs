namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        IGraphElementModel VerticesModel { get; }
        IGraphElementModel EdgesModel { get; }
        IGraphElementPropertiesModel PropertiesModel { get; }
    }
}
