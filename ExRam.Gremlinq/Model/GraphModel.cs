using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public static class GraphModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            public GraphModelImpl(IImmutableDictionary<Type, string> vertexLabels, IImmutableDictionary<Type, string> edgeTypes)
            {
                this.VertexLabels = vertexLabels;
                this.EdgeLabels = edgeTypes;
            }

            public IImmutableDictionary<Type, string> VertexLabels { get; }

            public IImmutableDictionary<Type, string> EdgeLabels { get; }
        }

        private sealed class VertexTypeInfoBuilder<T> : IVertexTypeInfoBuilder<T>
        {
            private readonly VertexTypeInfo _typeInfo;

            public VertexTypeInfoBuilder(VertexTypeInfo typeInfo)
            {
                this._typeInfo = typeInfo;
            }

            public VertexTypeInfo Build()
            {
                return this._typeInfo;
            }

            public IVertexTypeInfoBuilder<T> Label(string label)
            {
                return new VertexTypeInfoBuilder<T>(new VertexTypeInfo(this._typeInfo.ElementType, label));
            }
        }

        private sealed class EdgeTypeInfoBuilder<T> : IEdgeTypeInfoBuilder<T>
        {
            private readonly EdgeTypeInfo _typeInfo;

            public EdgeTypeInfoBuilder(EdgeTypeInfo typeInfo)
            {
                this._typeInfo = typeInfo;
            }

            public EdgeTypeInfo Build()
            {
                return this._typeInfo;
            }

            public IEdgeTypeInfoBuilder<T> Label(string label)
            {
                return new EdgeTypeInfoBuilder<T>(new EdgeTypeInfo(this._typeInfo.ElementType, label));
            }
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

            return new GraphModelImpl(
                assembly
                    .DefinedTypes
                    .Where(typeInfo => vertexBaseType.IsAssignableFrom(typeInfo.AsType()))
                    .ToImmutableDictionary(
                        type => type.AsType(),
                        type => namingStrategy.GetLabelForType(type.AsType())),
                assembly
                    .DefinedTypes
                    .Where(typeInfo => edgeBaseType.IsAssignableFrom(typeInfo.AsType()))
                    .ToImmutableDictionary(
                        type => type.AsType(),
                        type => namingStrategy.GetLabelForType(type.AsType())));
        }

        public static IGraphModel EdgeLabel<T>(IGraphModel model, string label)
        {
            var type = typeof(T);

            return model.VertexLabels.Keys
                .Where(vertexType => vertexType.IsAssignableFrom(type) || type.IsAssignableFrom(vertexType))
                .Select(_ => (Option<Type>)_)
                .FirstOrDefault()
                .Match(
                    contraditingVertexType => throw new ArgumentException($"Proposed edge type is inheritance hierarchy of vertex type {contraditingVertexType}."),
                    () => new GraphModelImpl(model.VertexLabels, model.EdgeLabels.SetItem(type, label)));
        }

        public static IGraphModel VertexLabel<T>(this IGraphModel model, string label)
        {
            var type = typeof(T);

            return model.EdgeLabels.Keys
                .Where(edgeType => edgeType.IsAssignableFrom(type) || type.IsAssignableFrom(edgeType))
                .Select(_ => (Option<Type>)_)
                .FirstOrDefault()
                .Match(
                    contraditingEdgeType => throw new ArgumentException($"Proposed vertex type is inheritance hierarchy of edge type {contraditingEdgeType}."),
                    () => new GraphModelImpl(model.VertexLabels.SetItem(type, label), model.EdgeLabels));
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

        internal static Option<Type> TryGetElementTypeOfLabel(this IEnumerable<KeyValuePair<Type, string>> elementInfos, string label)
        {
            return elementInfos
                .Where(elementInfo => elementInfo.Value.Equals(label, StringComparison.OrdinalIgnoreCase))
                .Select(elementInfo => elementInfo.Key)
                .FirstOrDefault();
        }

        public static IEnumerable<Type> GetDerivedElementInfos(this IGraphModel model, Type type, bool includeType)
        {
            return model.VertexLabels.Keys
                .Concat(model.EdgeLabels.Keys)
                .Where(kvp => !kvp.GetTypeInfo().IsAbstract && (includeType || kvp != type) && type.IsAssignableFrom(kvp));
        }
    }
}