using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        Type[] GetTypes(string label);

        IGraphElementModel VertexModel { get; }
        IGraphElementModel EdgeModel { get; }
    }
}
