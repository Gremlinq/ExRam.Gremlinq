using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public static class GremlinModel
    {
        private sealed class GremlinModelImpl : IGremlinModel
        {
            public GremlinModelImpl(IImmutableList<Type> vertexTypes, IImmutableList<Type> edgeTypes)
            {
                this.VertexTypes = vertexTypes;
                this.EdgeTypes = edgeTypes;
            }

            public IImmutableList<Type> VertexTypes { get; }

            public IImmutableList<Type> EdgeTypes { get; }
        }

        public static readonly IGremlinModel Empty = new GremlinModelImpl(ImmutableList<Type>.Empty, ImmutableList<Type>.Empty);

        public static IGremlinModel FromAssembly(Assembly assembly, Type vertexBaseType, Type edgeBaseType)
        {
            return new GremlinModelImpl(
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

        public static IGremlinModel AddVertexType<T>(this IGremlinModel model)
        {
            var type = typeof(T);

            if (model.VertexTypes.Contains(type))
                return model;

            var contraditingEdgeType = model.EdgeTypes.FirstOrDefault(edgeType => edgeType.IsAssignableFrom(type) || type.IsAssignableFrom(edgeType));
            if (contraditingEdgeType != null)
                throw new ArgumentException($"Proposed vertex type is inheritance hierarchy of edge type {contraditingEdgeType}.");

            return new GremlinModelImpl(model.VertexTypes.Add(typeof(T)), model.EdgeTypes);
        }

        public static IGremlinModel AddEdgeType<T>(this IGremlinModel model)
        {
            var type = typeof(T);

            if (model.EdgeTypes.Contains(type))
                return model;

            var contraditingVertexType = model.VertexTypes.FirstOrDefault(vertexType => vertexType.IsAssignableFrom(type) || type.IsAssignableFrom(vertexType));
            if (contraditingVertexType != null)
                throw new ArgumentException($"Proposed edge type is inheritance hierarchy of vertex type {contraditingVertexType}.");

            return new GremlinModelImpl(model.VertexTypes, model.EdgeTypes.Add(typeof(T)));
        }
    }
}