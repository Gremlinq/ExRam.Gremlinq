using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Dse
{
    public sealed class DseGraphModel : IGraphModel
    {
        public DseGraphModel(IImmutableDictionary<Type, VertexTypeInfo> vertexTypes, IImmutableDictionary<Type, EdgeTypeInfo> edgeTypes, IImmutableList<(Type, Type, Type)> connections)
        {
            this.VertexTypes = vertexTypes;
            this.EdgeTypes = edgeTypes;
            this.Connections = connections;
        }

        public IImmutableDictionary<Type, VertexTypeInfo> VertexTypes { get; }

        public IImmutableDictionary<Type, EdgeTypeInfo> EdgeTypes { get; }

        public IImmutableList<(Type, Type, Type)> Connections { get; }
    }
}