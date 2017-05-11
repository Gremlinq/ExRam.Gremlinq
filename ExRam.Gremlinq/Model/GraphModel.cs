using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public static class GremlinModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            public GraphModelImpl(IImmutableList<GraphElementInfo> vertexTypes, IImmutableList<GraphElementInfo> edgeTypes, IImmutableList<(GraphElementInfo, GraphElementInfo, GraphElementInfo)> connections)
            {
                this.VertexTypes = vertexTypes;
                this.EdgeTypes = edgeTypes;
                this.Connections = connections;
            }

            public IImmutableList<GraphElementInfo> VertexTypes { get; }

            public IImmutableList<GraphElementInfo> EdgeTypes { get; }

            public IImmutableList<(GraphElementInfo, GraphElementInfo, GraphElementInfo)> Connections { get; }
        }

        public static readonly IGraphModel Empty = new GraphModelImpl(ImmutableList<GraphElementInfo>.Empty, ImmutableList<GraphElementInfo>.Empty, ImmutableList<(GraphElementInfo, GraphElementInfo, GraphElementInfo)>.Empty);

        public static IGraphModel FromAssembly(Assembly assembly, Type vertexBaseType, Type edgeBaseType, IGraphElementNamingStrategy namingStrategy)
        {
            return new GraphModelImpl(
                assembly
                    .DefinedTypes
                    .Select(typeInfo => typeInfo.AsType())
                    .Where(vertexBaseType.IsAssignableFrom)
                    .Select(type => new GraphElementInfo(type, namingStrategy.GetLabelForType(type)))
                    .ToImmutableList(),
                assembly
                    .DefinedTypes
                    .Select(typeInfo => typeInfo.AsType())
                    .Where(edgeBaseType.IsAssignableFrom)
                    .Select(type => new GraphElementInfo(type, namingStrategy.GetLabelForType(type)))
                    .ToImmutableList(),
                ImmutableList<(GraphElementInfo, GraphElementInfo, GraphElementInfo)>.Empty);
        }

        public static IGraphModel AddVertexType<T>(this IGraphModel model, string label)
        {
            var type = typeof(T);

            if (model.VertexTypes.Any(typeInfo => typeInfo.ElementType == type))
                return model;

            var contraditingEdgeType = model.EdgeTypes
                .Where(edgeType => edgeType.ElementType.IsAssignableFrom(type) || type.IsAssignableFrom(edgeType.ElementType))
                .Cast<GraphElementInfo?>()
                .FirstOrDefault();

            if (contraditingEdgeType != null)
                throw new ArgumentException($"Proposed vertex type is inheritance hierarchy of edge type {contraditingEdgeType}.");

            return new GraphModelImpl(model.VertexTypes.Add(new GraphElementInfo(typeof(T), label)), model.EdgeTypes, model.Connections);
        }

        public static IGraphModel AddEdgeType<T>(this IGraphModel model, string label)
        {
            var type = typeof(T);

            if (model.EdgeTypes.Any(typeInfo => typeInfo.ElementType == type))
                return model;

            var contraditingVertexType = model.VertexTypes
                .Where(vertexType => vertexType.ElementType.IsAssignableFrom(type) || type.IsAssignableFrom(vertexType.ElementType))
                .Cast<GraphElementInfo?>()
                .FirstOrDefault();

            if (contraditingVertexType != null)
                throw new ArgumentException($"Proposed edge type is inheritance hierarchy of vertex type {contraditingVertexType}.");

            return new GraphModelImpl(model.VertexTypes, model.EdgeTypes.Add(new GraphElementInfo(typeof(T), label)), model.Connections);
        }

        public static IGraphModel AddConnection<TOutVertex, TEdge, TInVertex>(this IGraphModel model)
        {
            var outVertexInfo = model.TryGetVertexInfo(typeof(TOutVertex));

            if (outVertexInfo == null)
                throw new ArgumentException($"Model does not contain vertex type {typeof(TOutVertex)}.");

            var inVertexInfo = model.TryGetVertexInfo(typeof(TInVertex));

            if (inVertexInfo == null)
                throw new ArgumentException($"Model does not contain vertex type {typeof(TInVertex)}.");

            var edgeInfo = model.TryGetEdgeInfo(typeof(TEdge));

            if (edgeInfo == null)
                throw new ArgumentException($"Model does not contain edge type {typeof(TEdge)}.");

            var tuple = (outVertexInfo.GetValueOrDefault(), edgeInfo.GetValueOrDefault(), inVertexInfo.GetValueOrDefault());

            return model.Connections.Contains(tuple)
                ? model 
                : new GraphModelImpl(model.VertexTypes, model.EdgeTypes, model.Connections.Add(tuple));
        }

        public static Option<string> TryGetLabelOfType(this IGraphModel model, Type type)
        {
            return (model.TryGetVertexInfo(type) ?? model.TryGetEdgeInfo(type))?.Label;
        }

        public static Option<Type> TryGetVertexTypeOfLabel(this IGraphModel model, string label)
        {
            return model.VertexTypes
                .TryGetElementTypeOfLabel(label);
        }

        public static Option<Type> TryGetEdgeTypeOfLabel(this IGraphModel model, string label)
        {
            return model.EdgeTypes
                .TryGetElementTypeOfLabel(label);
        }

        public static Option<Type> TryGetElementTypeOfLabel(this IEnumerable<GraphElementInfo> elementInfos, string label)
        {
            return elementInfos
                .Where(elementInfo => elementInfo.Label.Equals(label, StringComparison.OrdinalIgnoreCase))
                .Select(elementInfo => elementInfo.ElementType)
                .FirstOrDefault();
        }

        private static GraphElementInfo? TryGetVertexInfo(this IGraphModel model, Type type)
        {
            return model.VertexTypes
                .TryGetElementInfo(type);
        }

        private static GraphElementInfo? TryGetEdgeInfo(this IGraphModel model, Type type)
        {
            return model.EdgeTypes
                .TryGetElementInfo(type);
        }

        private static GraphElementInfo? TryGetElementInfo(this IEnumerable<GraphElementInfo> infos, Type type)
        {
            return infos
                .Where(elementInfo => elementInfo.ElementType == type)
                .Cast<GraphElementInfo?>()
                .FirstOrDefault();
        }
    }
}