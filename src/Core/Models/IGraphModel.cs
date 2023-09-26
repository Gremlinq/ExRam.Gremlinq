using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphModel
    {
        IGraphModel ConfigureVertices(Func<IGraphElementModel, IGraphElementModel> transformation);
        IGraphModel ConfigureEdges(Func<IGraphElementModel, IGraphElementModel> transformation);

        IGraphModel AddAssemblies(params Assembly[] assemblies);

        IGraphElementModel VerticesModel { get; }
        IGraphElementModel EdgesModel { get; }
    }
}
