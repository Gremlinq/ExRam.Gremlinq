using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        Type[] GetTypes(string label);

        IGraphElementModel VerticesModel { get; }
        IGraphElementModel EdgesModel { get; }
    }
}
