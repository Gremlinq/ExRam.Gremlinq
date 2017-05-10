using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public static class GremlinModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            public GraphModelImpl(IImmutableList<Type> vertexTypes, IImmutableList<Type> edgeTypes, IImmutableList<(Type, Type, Type)> connections)
            {
                this.VertexTypes = vertexTypes;
                this.EdgeTypes = edgeTypes;
                this.Connections = connections;
            }

            public IImmutableList<Type> VertexTypes { get; }

            public IImmutableList<Type> EdgeTypes { get; }

            public IImmutableList<(Type, Type, Type)> Connections { get; }
        }

        public static readonly IGraphModel Empty = new GraphModelImpl(ImmutableList<Type>.Empty, ImmutableList<Type>.Empty, ImmutableList<(Type, Type, Type)>.Empty);

        public static IGraphModel FromAssembly(Assembly assembly, Type vertexBaseType, Type edgeBaseType)
        {
            return new GraphModelImpl(
                assembly
                    .DefinedTypes
                    .Select(typeInfo => typeInfo.AsType())
                    .Where(vertexBaseType.IsAssignableFrom)
                    .ToImmutableList(),
                assembly
                    .DefinedTypes
                    .Select(typeInfo => typeInfo.AsType())
                    .Where(edgeBaseType.IsAssignableFrom)
                    .ToImmutableList(),
                ImmutableList<(Type, Type, Type)>.Empty);
        }

        public static IGraphModel AddVertexType<T>(this IGraphModel model)
        {
            var type = typeof(T);

            if (model.VertexTypes.Contains(type))
                return model;

            var contraditingEdgeType = model.EdgeTypes.FirstOrDefault(edgeType => edgeType.IsAssignableFrom(type) || type.IsAssignableFrom(edgeType));
            if (contraditingEdgeType != null)
                throw new ArgumentException($"Proposed vertex type is inheritance hierarchy of edge type {contraditingEdgeType}.");

            return new GraphModelImpl(model.VertexTypes.Add(typeof(T)), model.EdgeTypes, model.Connections);
        }

        public static IGraphModel AddEdgeType<T>(this IGraphModel model)
        {
            var type = typeof(T);

            if (model.EdgeTypes.Contains(type))
                return model;

            var contraditingVertexType = model.VertexTypes.FirstOrDefault(vertexType => vertexType.IsAssignableFrom(type) || type.IsAssignableFrom(vertexType));
            if (contraditingVertexType != null)
                throw new ArgumentException($"Proposed edge type is inheritance hierarchy of vertex type {contraditingVertexType}.");

            return new GraphModelImpl(model.VertexTypes, model.EdgeTypes.Add(typeof(T)), model.Connections);
        }

        public static IGraphModel AddConnection<TOutVertex, TEdge, TInVertex>(this IGraphModel model)
        {
            var tuple = (typeof(TOutVertex), typeof(TEdge), typeof(TInVertex));

            if (model.Connections.Contains(tuple))
                return model;

            if (!model.VertexTypes.Contains(typeof(TOutVertex)))
                throw new ArgumentException($"Model does not contain vertex type {typeof(TOutVertex)}.");

            if (!model.VertexTypes.Contains(typeof(TInVertex)))
                throw new ArgumentException($"Model does not contain vertex type {typeof(TInVertex)}.");

            if (!model.EdgeTypes.Contains(typeof(TEdge)))
                throw new ArgumentException($"Model does not contain edge type {typeof(TEdge)}.");

            return new GraphModelImpl(model.VertexTypes, model.EdgeTypes, model.Connections.Add(tuple));
        }
    }
}