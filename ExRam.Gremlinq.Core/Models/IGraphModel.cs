using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        IGraphElementModel VerticesModel { get; }
        IGraphElementModel EdgesModel { get; }
        IGraphElementPropertyModel PropertiesModel { get; }

        IGraphModel ConfigureVertices(Func<IGraphElementModel, IGraphElementModel> transformation);
        IGraphModel ConfigureEdges(Func<IGraphElementModel, IGraphElementModel> transformation);
        IGraphModel ConfigureProperties(Func<IGraphElementPropertyModel, IGraphElementPropertyModel> transformation);
    }
}
