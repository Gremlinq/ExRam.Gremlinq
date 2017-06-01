using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public static class GremlinModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            public GraphModelImpl(IImmutableList<VertexInfo> vertexTypes, IImmutableList<EdgeInfo> edgeTypes, IImmutableList<(VertexInfo, EdgeInfo, VertexInfo)> connections)
            {
                this.VertexTypes = vertexTypes;
                this.EdgeTypes = edgeTypes;
                this.Connections = connections;
            }

            public IImmutableList<VertexInfo> VertexTypes { get; }

            public IImmutableList<EdgeInfo> EdgeTypes { get; }

            public IImmutableList<(VertexInfo, EdgeInfo, VertexInfo)> Connections { get; }
        }

        public static readonly IGraphModel Empty = new GraphModelImpl(ImmutableList<VertexInfo>.Empty, ImmutableList<EdgeInfo>.Empty, ImmutableList<(VertexInfo, EdgeInfo, VertexInfo)>.Empty);

        public static IGraphModel FromAssembly<TVertex, TEdge>(Assembly assembly, IGraphElementNamingStrategy namingStrategy)
        {
            return FromAssembly(assembly, typeof(TVertex), typeof(TEdge), namingStrategy);
        }

        public static IGraphModel FromAssembly(Assembly assembly, Type vertexBaseType, Type edgeBaseType, IGraphElementNamingStrategy namingStrategy)
        {
            return new GraphModelImpl(
                assembly
                    .DefinedTypes
                    .Where(typeInfo => vertexBaseType.IsAssignableFrom(typeInfo.AsType()))
                    .Select(type => new VertexInfo(type.AsType(), namingStrategy.GetLabelForType(type.AsType())))
                    .ToImmutableList(),
                assembly
                    .DefinedTypes
                    .Where(typeInfo => edgeBaseType.IsAssignableFrom(typeInfo.AsType()))
                    .Select(type => new EdgeInfo(type.AsType(), namingStrategy.GetLabelForType(type.AsType())))
                    .ToImmutableList(),
                ImmutableList<(VertexInfo, EdgeInfo, VertexInfo)>.Empty);
        }

        public static IGraphModel AddVertexType<T>(this IGraphModel model, string label)
        {
            var type = typeof(T);

            if (model.VertexTypes.Any(typeInfo => typeInfo.ElementType == type))
                return model;

            return model.EdgeTypes
                .Where(edgeType => edgeType.ElementType.IsAssignableFrom(type) || type.IsAssignableFrom(edgeType.ElementType))
                .Select(_ => (Option<EdgeInfo>)_)
                .FirstOrDefault()
                .Match(
                    contraditingEdgeType => throw new ArgumentException($"Proposed vertex type is inheritance hierarchy of edge type {contraditingEdgeType}."),
                    () => new GraphModelImpl(model.VertexTypes.Add(new VertexInfo(typeof(T), label)), model.EdgeTypes, model.Connections));
        }

        public static IGraphModel AddEdgeType<T>(this IGraphModel model, string label)
        {
            var type = typeof(T);

            if (model.EdgeTypes.Any(typeInfo => typeInfo.ElementType == type))
                return model;

            return model.VertexTypes
                .Where(vertexType => vertexType.ElementType.IsAssignableFrom(type) || type.IsAssignableFrom(vertexType.ElementType))
                .Select(_ => (Option<VertexInfo>)_)
                .FirstOrDefault()
                .Match(
                    contraditingVertexType => throw new ArgumentException($"Proposed edge type is inheritance hierarchy of vertex type {contraditingVertexType}."),
                    () => new GraphModelImpl(model.VertexTypes, model.EdgeTypes.Add(new EdgeInfo(typeof(T), label)), model.Connections));
        }

        public static IGraphModel AddConnection<TOutVertex, TEdge, TInVertex>(this IGraphModel model)
        {
            return model.AddConnection(typeof(TOutVertex), typeof(TEdge), typeof(TInVertex));
        }

        private static IGraphModel AddConnection(this IGraphModel model, Type outVertexType, Type edgeType, Type inVertexType)
        {
            var outVertexInfo = model
                .TryGetVertexInfo(outVertexType)
                .IfNone(() => throw new ArgumentException($"Model does not contain vertex type {outVertexType}."));

            var inVertexInfo = model
                .TryGetVertexInfo(inVertexType)
                .IfNone(() => throw new ArgumentException($"Model does not contain vertex type {inVertexType}."));

            var edgeInfo = model
                .TryGetEdgeInfo(edgeType)
                .IfNone(() => throw new ArgumentException($"Model does not contain edge type {edgeType}."));

            var tuple = (outVertexInfo, edgeInfo, inVertexInfo);

            return model.Connections.Contains(tuple)
                ? model
                : new GraphModelImpl(model.VertexTypes, model.EdgeTypes, model.Connections.Add(tuple));
        }

        public static IGraphModel EdgeConnectionClosure(this IGraphModel model)
        {
            foreach (var connection in model.Connections)
            {
                foreach (var outVertexClosure in model.GetDerivedElementInfos(connection.Item1.ElementType, true))
                {
                    foreach (var edgeClosure in model.GetDerivedElementInfos(connection.Item2.ElementType, true))
                    {
                        foreach (var inVertexClosure in model.GetDerivedElementInfos(connection.Item3.ElementType, true))
                        {
                            model = model.AddConnection(outVertexClosure.ElementType, edgeClosure.ElementType, inVertexClosure.ElementType);
                        }
                    }
                }
            }

            return model;
        }

        public static Option<string> TryGetLabelOfType(this IGraphModel model, Type type)
        {
            return model
                .TryGetVertexInfo(type)
                .Match(
                    _ => _,
                    () => model
                        .TryGetEdgeInfo(type)
                        .Map(_ => (GraphElementInfo) _))
                .Map(_ => _.Label);
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

        internal static IEnumerable<GraphElementInfo> GetDerivedElementInfos(this IGraphModel model, Type type, bool includeType)
        {
            return model.VertexTypes
                .Cast<GraphElementInfo>()
                .Concat(model.EdgeTypes)
                .Where(elementInfo => (includeType || elementInfo.ElementType != type) && type.IsAssignableFrom(elementInfo.ElementType));
        }

        private static Option<VertexInfo> TryGetVertexInfo(this IGraphModel model, Type type)
        {
            return model.VertexTypes
                .TryGetElementInfo(type)
                .Map(_ => (VertexInfo)_);
        }

        private static Option<EdgeInfo> TryGetEdgeInfo(this IGraphModel model, Type type)
        {
            return model.EdgeTypes
                .TryGetElementInfo(type)
                .Map(_ => (EdgeInfo)_);
        }

        private static Option<GraphElementInfo> TryGetElementInfo(this IEnumerable<GraphElementInfo> infos, Type type)
        {
            return infos
                .Where(elementInfo => elementInfo.ElementType == type)
                .Select(_ => (Option<GraphElementInfo>)_)
                .FirstOrDefault();
        }
    }
}