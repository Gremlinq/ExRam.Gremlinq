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
            public GraphModelImpl(IImmutableDictionary<Type, VertexInfo> vertexTypes, IImmutableDictionary<Type, EdgeInfo> edgeTypes, IImmutableList<(Type, Type, Type)> connections)
            {
                this.VertexTypes = vertexTypes;
                this.EdgeTypes = edgeTypes;
                this.Connections = connections;
            }

            public IImmutableDictionary<Type, VertexInfo> VertexTypes { get; }

            public IImmutableDictionary<Type, EdgeInfo> EdgeTypes { get; }

            public IImmutableList<(Type, Type, Type)> Connections { get; }
        }

        public static readonly IGraphModel Empty = new GraphModelImpl(ImmutableDictionary<Type, VertexInfo>.Empty, ImmutableDictionary<Type, EdgeInfo>.Empty, ImmutableList<(Type, Type, Type)>.Empty);

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
                    .ToImmutableDictionary(
                        type => type.AsType(),
                        type => new VertexInfo(type.AsType(), namingStrategy.GetLabelForType(type.AsType()))),
                assembly
                    .DefinedTypes
                    .Where(typeInfo => edgeBaseType.IsAssignableFrom(typeInfo.AsType()))
                    .ToImmutableDictionary(
                        type => type.AsType(),
                        type => new EdgeInfo(type.AsType(), namingStrategy.GetLabelForType(type.AsType()))),
                ImmutableList<(Type, Type, Type)>.Empty);
        }

        public static IGraphModel AddVertexType<T>(this IGraphModel model, string label)
        {
            var type = typeof(T);

            if (model.VertexTypes.ContainsKey(type))
                return model;

            return model.EdgeTypes.Keys
                .Where(edgeType => edgeType.IsAssignableFrom(type) || type.IsAssignableFrom(edgeType))
                .Select(_ => (Option<Type>)_)
                .FirstOrDefault()
                .Match(
                    contraditingEdgeType => throw new ArgumentException($"Proposed vertex type is inheritance hierarchy of edge type {contraditingEdgeType}."),
                    () => new GraphModelImpl(model.VertexTypes.Add(type, new VertexInfo(typeof(T), label)), model.EdgeTypes, model.Connections));
        }

        public static IGraphModel AddEdgeType<T>(this IGraphModel model, string label)
        {
            var type = typeof(T);

            if (model.EdgeTypes.ContainsKey(type))
                return model;

            return model.VertexTypes.Keys
                .Where(vertexType => vertexType.IsAssignableFrom(type) || type.IsAssignableFrom(vertexType))
                .Select(_ => (Option<Type>)_)
                .FirstOrDefault()
                .Match(
                    contraditingVertexType => throw new ArgumentException($"Proposed edge type is inheritance hierarchy of vertex type {contraditingVertexType}."),
                    () => new GraphModelImpl(model.VertexTypes, model.EdgeTypes.Add(type, new EdgeInfo(typeof(T), label)), model.Connections));
        }

        public static IGraphModel AddConnection<TOutVertex, TEdge, TInVertex>(this IGraphModel model)
        {
            return model.AddConnection(typeof(TOutVertex), typeof(TEdge), typeof(TInVertex));
        }

        private static IGraphModel AddConnection(this IGraphModel model, Type outVertexType, Type edgeType, Type inVertexType)
        {
            var outVertexInfo = model
                .TryGetVertexInfo(outVertexType)
                .Map(vertexInfo => vertexInfo.ElementType)
                .IfNone(() => throw new ArgumentException($"Model does not contain vertex type {outVertexType}."));

            var inVertexInfo = model
                .TryGetVertexInfo(inVertexType)
                .Map(vertexInfo => vertexInfo.ElementType)
                .IfNone(() => throw new ArgumentException($"Model does not contain vertex type {inVertexType}."));

            var connectionEdgeInfo = model
                .TryGetEdgeInfo(edgeType)
                .Map(edgeInfo => edgeInfo.ElementType)
                .IfNone(() => throw new ArgumentException($"Model does not contain edge type {edgeType}."));

            var tuple = (outVertexInfo, connectionEdgeInfo, inVertexInfo);

            return model.Connections.Contains(tuple)
                ? model
                : new GraphModelImpl(model.VertexTypes, model.EdgeTypes, model.Connections.Add(tuple));
        }

        public static IGraphModel EdgeConnectionClosure(this IGraphModel model)
        {
            foreach (var connection in model.Connections)
            {
                foreach (var outVertexClosure in model.GetDerivedElementInfos(connection.Item1, true))
                {
                    foreach (var edgeClosure in model.GetDerivedElementInfos(connection.Item2, true))
                    {
                        foreach (var inVertexClosure in model.GetDerivedElementInfos(connection.Item3, true))
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
            return model.VertexTypes.Values
                .TryGetElementTypeOfLabel(label);
        }

        public static Option<Type> TryGetEdgeTypeOfLabel(this IGraphModel model, string label)
        {
            return model.EdgeTypes.Values
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
            return model.VertexTypes.Values
                .Cast<GraphElementInfo>()
                .Concat(model.EdgeTypes.Values)
                .Where(elementInfo => (includeType || elementInfo.ElementType != type) && type.IsAssignableFrom(elementInfo.ElementType));
        }

        private static Option<VertexInfo> TryGetVertexInfo(this IGraphModel model, Type type)
        {
            return model.VertexTypes
                .TryGetValue(type);
        }

        private static Option<EdgeInfo> TryGetEdgeInfo(this IGraphModel model, Type type)
        {
            return model.EdgeTypes
                .TryGetValue(type);
        }
    }
}