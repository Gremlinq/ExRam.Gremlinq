using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        IGraphElementModel VerticesModel { get; }
        IGraphElementModel EdgesModel { get; }
        IGraphElementPropertyModel PropertiesModel { get; }
        IImmutableSet<Type> NativeTypes { get; }

        IGraphModel ConfigureNativeTypes(Func<IImmutableSet<Type>, IImmutableSet<Type>> transformation);
        IGraphModel ConfigureVertices(Func<IGraphElementModel, IGraphElementModel> transformation);
        IGraphModel ConfigureEdges(Func<IGraphElementModel, IGraphElementModel> transformation);
        IGraphModel ConfigureProperties(Func<IGraphElementPropertyModel, IGraphElementPropertyModel> transformation);
    }
}
