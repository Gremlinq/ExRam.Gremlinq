using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphModel
    {
        IGraphModel AddAssemblies(params Assembly[] assemblies);

        IGraphModel ConfigureEdges(Func<IGraphElementModel, IGraphElementModel> transformation);
        IGraphModel ConfigureVertices(Func<IGraphElementModel, IGraphElementModel> transformation);

        IGraphElementModel EdgesModel { get; }
        IGraphElementModel VerticesModel { get; }
    }
}
