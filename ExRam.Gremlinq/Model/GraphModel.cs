using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public static class GraphModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            public GraphModelImpl(IImmutableDictionary<Type, string> vertexLabels, IImmutableDictionary<Type, string> edgeTypes, Option<string> idPropertyName = default)
            {
                this.VertexLabels = vertexLabels;
                this.EdgeLabels = edgeTypes;
                this.IdPropertyName = idPropertyName;
            }

            public Option<string> IdPropertyName { get; }

            public IImmutableDictionary<Type, string> VertexLabels { get; }

            public IImmutableDictionary<Type, string> EdgeLabels { get; }
        }

        public static readonly IGraphModel Empty = new GraphModelImpl(ImmutableDictionary<Type, string>.Empty, ImmutableDictionary<Type, string>.Empty);

        public static IGraphModel FromAssembly<TVertex, TEdge>(Assembly assembly, IGraphElementNamingStrategy namingStrategy)
        {
            return FromAssembly(assembly, typeof(TVertex), typeof(TEdge), namingStrategy);
        }

        public static IGraphModel FromAssembly(Assembly assembly, Type vertexBaseType, Type edgeBaseType, IGraphElementNamingStrategy namingStrategy)
        {
            if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

            if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

            return GraphModel.Empty
                .AddVertexTypes(assembly
                    .DefinedTypes
                    .Where(typeInfo => vertexBaseType.IsAssignableFrom(typeInfo.AsType()))
                    .Select(typeInfo => typeInfo.AsType())
                    .ToImmutableDictionary(
                        type => type,
                        namingStrategy.GetLabelForType))
                .AddEdgeTypes(assembly
                    .DefinedTypes
                    .Where(typeInfo => edgeBaseType.IsAssignableFrom(typeInfo.AsType()))
                    .Select(typeInfo => typeInfo.AsType())
                    .ToImmutableDictionary(
                        type => type,
                        namingStrategy.GetLabelForType));
        }

        private static IGraphModel AddVertexTypes(this IGraphModel model, IEnumerable<KeyValuePair<Type, string>> types)
        {
            return new GraphModelImpl(
                model.VertexLabels.AddRange(types),
                model.EdgeLabels,
                model.IdPropertyName);
        }

        private static IGraphModel AddEdgeTypes(this IGraphModel model, IEnumerable<KeyValuePair<Type, string>> types)
        {
            return new GraphModelImpl(
                model.VertexLabels,
                model.EdgeLabels.AddRange(types),
                model.IdPropertyName);
        }

        public static IGraphModel EdgeLabel<TElement>(this IGraphModel model, string label = null)
        {
            var type = typeof(TElement);

            return model.VertexLabels.Keys
                .Where(vertexType => vertexType.IsAssignableFrom(type) || type.IsAssignableFrom(vertexType))
                .Select(_ => (Option<Type>)_)
                .FirstOrDefault()
                .Match(
                    contraditingVertexType => throw new ArgumentException($"Proposed edge type is inheritance hierarchy of vertex type {contraditingVertexType}."),
                    () => new GraphModelImpl(model.VertexLabels, model.EdgeLabels.SetItem(type, label ?? typeof(TElement).Name), model.IdPropertyName));
        }

        public static IGraphModel WithIdPropertyName(this IGraphModel model, string idPropertyName)
        {
            return new GraphModelImpl(
                model.VertexLabels,
                model.EdgeLabels,
                idPropertyName);
        }

        public static IGraphModel VertexLabel<TElement>(this IGraphModel model, string label = null)
        {
            var type = typeof(TElement);

            return model.EdgeLabels.Keys
                .Where(edgeType => edgeType.IsAssignableFrom(type) || type.IsAssignableFrom(edgeType))
                .Select(_ => (Option<Type>)_)
                .FirstOrDefault()
                .Match(
                    contraditingEdgeType => throw new ArgumentException($"Proposed vertex type is inheritance hierarchy of edge type {contraditingEdgeType}."),
                    () => new GraphModelImpl(model.VertexLabels.SetItem(type, label ?? typeof(TElement).Name), model.EdgeLabels, model.IdPropertyName));
        }

        public static string GetLabelOfType(this IGraphModel model, Type type)
        {
            return model
                .TryGetLabelOfType(type)
                .IfNone(() => throw new ArgumentException($"Cannot find label of type {type} in model."));
        }

        public static Option<string> TryGetLabelOfType(this IGraphModel model, Type type)
        {
            return model.VertexLabels
                .TryGetValue(type)
                .Match(
                    _ => _,
                    () => model.EdgeLabels
                        .TryGetValue(type));
        }

        internal static Option<Type> TryGetVertexTypeOfLabel(this IGraphModel model, string label)
        {
            return model.VertexLabels
                .TryGetElementTypeOfLabel(label);
        }

        internal static Option<Type> TryGetEdgeTypeOfLabel(this IGraphModel model, string label)
        {
            return model.EdgeLabels
                .TryGetElementTypeOfLabel(label);
        }

        internal static Option<Type> TryGetElementTypeOfLabel(this IGraphModel model, string label)
        {
            return model.VertexLabels
                .Concat(model.EdgeLabels)
                .TryGetElementTypeOfLabel(label);
        }

        internal static Option<Type> TryGetElementTypeOfLabel(this IEnumerable<KeyValuePair<Type, string>> elementInfos, string label)
        {
            return elementInfos
                .Where(elementInfo => elementInfo.Value.Equals(label, StringComparison.OrdinalIgnoreCase))
                .Select(elementInfo => elementInfo.Key)
                .FirstOrDefault();
        }

        public static IEnumerable<Type> GetDerivedTypes(this IGraphModel model, Type type, bool includeType)
        {
            return model.VertexLabels.Keys
                .Concat(model.EdgeLabels.Keys)
                .Where(kvp => !kvp.GetTypeInfo().IsAbstract && (includeType || kvp != type) && type.IsAssignableFrom(kvp));
        }
    }
}