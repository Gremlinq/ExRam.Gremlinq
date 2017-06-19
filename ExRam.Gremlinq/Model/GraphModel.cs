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
            public GraphModelImpl(IImmutableDictionary<Type, VertexTypeInfo> vertexTypes, IImmutableDictionary<Type, EdgeTypeInfo> edgeTypes)
            {
                this.VertexTypes = vertexTypes;
                this.EdgeTypes = edgeTypes;
            }

            public IImmutableDictionary<Type, VertexTypeInfo> VertexTypes { get; }

            public IImmutableDictionary<Type, EdgeTypeInfo> EdgeTypes { get; }
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

        public static readonly IGraphModel Empty = new GraphModelImpl(ImmutableDictionary<Type, VertexTypeInfo>.Empty, ImmutableDictionary<Type, EdgeTypeInfo>.Empty);

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
                        type => new VertexTypeInfo(type.AsType(), namingStrategy.GetLabelForType(type.AsType()))),
                assembly
                    .DefinedTypes
                    .Where(typeInfo => edgeBaseType.IsAssignableFrom(typeInfo.AsType()))
                    .ToImmutableDictionary(
                        type => type.AsType(),
                        type => new EdgeTypeInfo(type.AsType(), namingStrategy.GetLabelForType(type.AsType()))));
        }

        public static IGraphModel EdgeType<T>(this IGraphModel model, Func<IEdgeTypeInfoBuilder<T>, IEdgeTypeInfoBuilder<T>> builderAction)
        {
            var type = typeof(T);

            var edgeInfo = model.EdgeTypes
                .TryGetValue(type)
                .IfNone(new EdgeTypeInfo(type, null));

            return model.VertexTypes.Keys
                .Where(vertexType => vertexType.IsAssignableFrom(type) || type.IsAssignableFrom(vertexType))
                .Select(_ => (Option<Type>)_)
                .FirstOrDefault()
                .Match(
                    contraditingVertexType => throw new ArgumentException($"Proposed edge type is inheritance hierarchy of vertex type {contraditingVertexType}."),
                    () => new GraphModelImpl(model.VertexTypes, model.EdgeTypes.SetItem(type, builderAction(new EdgeTypeInfoBuilder<T>(edgeInfo)).Build())));
        }

        public static IGraphModel VertexType<T>(this IGraphModel model, Func<IVertexTypeInfoBuilder<T>, IVertexTypeInfoBuilder<T>> builderAction)
        {
            var type = typeof(T);

            var vertexInfo = model.VertexTypes
                .TryGetValue(type)
                .IfNone(new VertexTypeInfo(type, null));

            return model.EdgeTypes.Keys
                .Where(edgeType => edgeType.IsAssignableFrom(type) || type.IsAssignableFrom(edgeType))
                .Select(_ => (Option<Type>)_)
                .FirstOrDefault()
                .Match(
                    contraditingEdgeType => throw new ArgumentException($"Proposed vertex type is inheritance hierarchy of edge type {contraditingEdgeType}."),
                    () => new GraphModelImpl(model.VertexTypes.SetItem(type, builderAction(new VertexTypeInfoBuilder<T>(vertexInfo)).Build()), model.EdgeTypes));
        }

        public static Option<string> TryGetLabelOfType(this IGraphModel model, Type type)
        {
            return model.VertexTypes
                .TryGetValue(type)
                .Match(
                    _ => _,
                    () => model.EdgeTypes
                        .TryGetValue(type)
                        .Map(_ => (GraphElementInfo) _))
                .Map(_ => _.Label);
        }

        internal static Option<Type> TryGetVertexTypeOfLabel(this IGraphModel model, string label)
        {
            return model.VertexTypes.Values
                .TryGetElementTypeOfLabel(label);
        }

        internal static Option<Type> TryGetEdgeTypeOfLabel(this IGraphModel model, string label)
        {
            return model.EdgeTypes.Values
                .TryGetElementTypeOfLabel(label);
        }

        internal static Option<Type> TryGetElementTypeOfLabel(this IEnumerable<GraphElementInfo> elementInfos, string label)
        {
            return elementInfos
                .Where(elementInfo => elementInfo.Label.Equals(label, StringComparison.OrdinalIgnoreCase))
                .Select(elementInfo => elementInfo.ElementType)
                .FirstOrDefault();
        }

        public static IEnumerable<GraphElementInfo> GetDerivedElementInfos(this IGraphModel model, Type type, bool includeType)
        {
            return model.VertexTypes.Values
                .Cast<GraphElementInfo>()
                .Concat(model.EdgeTypes.Values)
                .Where(elementInfo => !elementInfo.ElementType.GetTypeInfo().IsAbstract && (includeType || elementInfo.ElementType != type) && type.IsAssignableFrom(elementInfo.ElementType));
        }
    }
}