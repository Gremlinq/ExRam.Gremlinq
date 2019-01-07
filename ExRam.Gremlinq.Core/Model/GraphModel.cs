using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class GraphModel
    {
        private sealed class EmptyGraphModel : IGraphModel
        {
            public Option<string> TryGetConstructiveVertexLabel(Type elementType) => default;

            public Option<string> TryGetConstructiveEdgeLabel(Type elementType) => default;

            public Option<string[]> TryGetVertexFilterLabels(Type elementType) => default;

            public Option<string[]> TryGetEdgeFilterLabels(Type elementType) => default;

            public Type[] GetTypes(string label) => Array.Empty<Type>();

            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public Option<string> EdgeIdPropertyName { get; }

            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public Option<string> VertexIdPropertyName { get; }
        }

        private sealed class InvalidGraphModel : IGraphModel
        {
            public Option<string> TryGetConstructiveVertexLabel(Type elementType) => throw new InvalidOperationException();

            public Option<string> TryGetConstructiveEdgeLabel(Type elementType) => throw new InvalidOperationException();

            public Option<string[]> TryGetVertexFilterLabels(Type elementType) => throw new InvalidOperationException();

            public Option<string[]> TryGetEdgeFilterLabels(Type elementType) => throw new InvalidOperationException();
            
            public Type[] GetTypes(string label) => throw new InvalidOperationException();

            public Option<string> EdgeIdPropertyName { get => throw new InvalidOperationException(); }

            public Option<string> VertexIdPropertyName { get => throw new InvalidOperationException(); }
        }

        private sealed class AssemblyGraphModelImpl : IGraphModel
        {
            private readonly Type _edgeBaseType;
            private readonly Type _vertexBaseType;
            private readonly IDictionary<string, Type[]> _types;
            private readonly IDictionary<Type, string[]> _edgeLabels;
            private readonly IDictionary<Type, string[]> _vertexLabels;

            private readonly ConcurrentDictionary<Type, string[]> _derivedVertexLabels = new ConcurrentDictionary<Type, string[]>();
            private readonly ConcurrentDictionary<Type, string[]> _derivedEdgeLabels = new ConcurrentDictionary<Type, string[]>();

            public AssemblyGraphModelImpl(Type vertexBaseType, Type edgeBaseType, string vertexIdPropertyName, string edgeIdPropertyName, IEnumerable<Assembly> assemblies, ILogger logger)
            {
                if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                    throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

                if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                    throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

                _vertexBaseType = vertexBaseType;
                _edgeBaseType = edgeBaseType;

                _vertexLabels = GetLabels(assemblies, vertexBaseType, logger);
                _edgeLabels = GetLabels(assemblies, edgeBaseType, logger);

                _types = _vertexLabels
                    .Concat(_edgeLabels)
                    .GroupBy(x => x.Value[0])
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(x => x.Key)
                            .ToArray());

                VertexIdPropertyName = vertexIdPropertyName;
                EdgeIdPropertyName = edgeIdPropertyName;
            }

            public Option<string> TryGetConstructiveVertexLabel(Type elementType)
            {
                return _vertexLabels
                    .TryGetValue(elementType)
                    .Map(x => x.FirstOrDefault())
                    .IfNone(() => elementType.BaseType != null
                        ? TryGetConstructiveVertexLabel(elementType.BaseType)
                        : default);
            }

            public Option<string> TryGetConstructiveEdgeLabel(Type elementType)
            {
                return _edgeLabels
                    .TryGetValue(elementType)
                    .Map(x => x.FirstOrDefault())
                    .IfNone(() => elementType.BaseType != null
                        ? TryGetConstructiveVertexLabel(elementType.BaseType)
                        : default);
            }

            public Option<string[]> TryGetVertexFilterLabels(Type elementType)
            {
                return elementType.IsAssignableFrom(_vertexBaseType)
                    ? default
                    : TryGetFilterLabels(_vertexLabels, _derivedVertexLabels, elementType);
            }

            public Option<string[]> TryGetEdgeFilterLabels(Type elementType)
            {
                return elementType.IsAssignableFrom(_edgeBaseType)
                    ? default
                    : TryGetFilterLabels(_edgeLabels, _derivedEdgeLabels, elementType);
            }

            private Option<string[]> TryGetFilterLabels(IDictionary<Type, string[]> labels, ConcurrentDictionary<Type, string[]> derivedLabels, Type elementType)
            {
                return derivedLabels.GetOrAdd(
                    elementType,
                    closureType => labels
                        .Where(kvp => !kvp.Key.GetTypeInfo().IsAbstract && closureType.IsAssignableFrom(kvp.Key))
                        .Select(kvp => kvp.Value[0])
                        .OrderBy(x => x)
                        .ToArray());
            }

            private IDictionary<Type, string[]> GetLabels(IEnumerable<Assembly> assemblies, Type baseType, ILogger logger)
            {
                return assemblies
                    .Distinct()
                    .SelectMany(assembly =>
                    {
                        try
                        {
                            return assembly
                                .DefinedTypes
                                .Where(type => type != baseType && baseType.IsAssignableFrom(type))
                                .Select(typeInfo => typeInfo);
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            logger?.LogWarning(ex, $"{nameof(ReflectionTypeLoadException)} thrown during GraphModel creation.");
                            return Array.Empty<TypeInfo>();
                        }
                    })
                    .Prepend(baseType)
                    .Where(x => !x.IsInterface)
                    .ToDictionary(
                        type => type,
                        type => new[] { type.Name });
            }

            public Type[] GetTypes(string label)
            {
                return _types
                    .TryGetValue(label)
                    .IfNone(Array.Empty<Type>());
            }

            public Option<string> VertexIdPropertyName { get; }
            public Option<string> EdgeIdPropertyName { get; }
        }

        public static readonly IGraphModel Empty = new EmptyGraphModel();
        public static readonly IGraphModel Invalid = new InvalidGraphModel();

        public static IGraphModel Dynamic(ILogger logger = null)
        {
            return FromAssemblies<IVertex, IEdge>(x => x.Id, x => x.Id, logger, AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IGraphModel FromBaseTypes<TVertex, TEdge>(Expression<Func<TVertex, object>> vertexId, Expression<Func<TVertex, object>> edgeId, ILogger logger = null)
        {
            return FromAssemblies<TVertex, TEdge>(vertexId, edgeId, logger, typeof(TVertex).Assembly, typeof(TEdge).Assembly);
        }

        public static IGraphModel FromExecutingAssembly(ILogger logger = null)
        {
            return FromAssemblies<IVertex, IEdge>(x => x.Id, x => x.Id, logger, Assembly.GetCallingAssembly());
        }

        public static IGraphModel FromExecutingAssembly<TVertex, TEdge>(Expression<Func<TVertex, object>> vertexId, Expression<Func<TVertex, object>> edgeId, ILogger logger = null)
        {
            return FromAssemblies<TVertex, TEdge>(vertexId, edgeId, logger, Assembly.GetCallingAssembly());
        }

        public static IGraphModel FromAssemblies<TVertex, TEdge>(Expression<Func<TVertex, object>> vertexId, Expression<Func<TVertex, object>> edgeId, ILogger logger = null, params Assembly[] assemblies)
        {
            return FromAssemblies(typeof(TVertex), typeof(TEdge), ((MemberExpression)vertexId.Body.StripConvert()).Member.Name, ((MemberExpression)edgeId.Body.StripConvert()).Member.Name, logger, assemblies);
        }

        public static IGraphModel FromAssemblies(Type vertexBaseType, Type edgeBaseType, string vertexIdPropertyName = "Id", string edgeIdPropertyName = "Id", ILogger logger = null, params Assembly[] assemblies)
        {
            return new AssemblyGraphModelImpl(vertexBaseType, edgeBaseType, vertexIdPropertyName, edgeIdPropertyName, assemblies, logger);
        }

        internal static string[] GetValidVertexFilterLabels(this IGraphModel model, Type type)
        {
            return GetValidFilterLabels(type, model.TryGetVertexFilterLabels(type));
        }

        internal static string[] GetValidEdgeFilterLabels(this IGraphModel model, Type type)
        {
            return GetValidFilterLabels(type, model.TryGetEdgeFilterLabels(type));
        }

        private static string[] GetValidFilterLabels(Type type, Option<string[]> maybeLabels)
        {
            return maybeLabels
                .Filter(labels =>
                {
                    if (labels.Length == 0)
                        throw new InvalidOperationException($"Can't determine labels for type {type.FullName}.");

                    return true;
                })
                .IfNone(Array.Empty<string>());
        }

        internal static object GetIdentifier(this IGraphModel model, GraphElementType elementType, string name)
        {
            return elementType == GraphElementType.Vertex && name == model.VertexIdPropertyName
                || elementType == GraphElementType.Edge && name == model.EdgeIdPropertyName
                ? (object)T.Id
                : name;
        }
    }
}
