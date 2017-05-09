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
            public GraphModelImpl(IImmutableList<Type> vertexTypes, IImmutableList<Type> edgeTypes)
            {
                this.VertexTypes = vertexTypes;
                this.EdgeTypes = edgeTypes;
            }

            public IImmutableList<Type> VertexTypes { get; }

            public IImmutableList<Type> EdgeTypes { get; }
        }

        public static readonly IGraphModel Empty = new GraphModelImpl(ImmutableList<Type>.Empty, ImmutableList<Type>.Empty);

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
                    .ToImmutableList());
        }

        public static IGraphModel AddVertexType<T>(this IGraphModel model)
        {
            var type = typeof(T);

            if (model.VertexTypes.Contains(type))
                return model;

            var contraditingEdgeType = model.EdgeTypes.FirstOrDefault(edgeType => edgeType.IsAssignableFrom(type) || type.IsAssignableFrom(edgeType));
            if (contraditingEdgeType != null)
                throw new ArgumentException($"Proposed vertex type is inheritance hierarchy of edge type {contraditingEdgeType}.");

            return new GraphModelImpl(model.VertexTypes.Add(typeof(T)), model.EdgeTypes);
        }

        public static IGraphModel AddEdgeType<T>(this IGraphModel model)
        {
            var type = typeof(T);

            if (model.EdgeTypes.Contains(type))
                return model;

            var contraditingVertexType = model.VertexTypes.FirstOrDefault(vertexType => vertexType.IsAssignableFrom(type) || type.IsAssignableFrom(vertexType));
            if (contraditingVertexType != null)
                throw new ArgumentException($"Proposed edge type is inheritance hierarchy of vertex type {contraditingVertexType}.");

            return new GraphModelImpl(model.VertexTypes, model.EdgeTypes.Add(typeof(T)));
        }
    }
}