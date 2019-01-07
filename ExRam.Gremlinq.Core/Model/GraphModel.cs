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
            public Type[] GetTypes(string label) => Array.Empty<Type>();

            public IGraphElementModel VerticesModel { get => GraphElementModel.Empty; }
            public IGraphElementModel EdgesModel { get => GraphElementModel.Empty; }
        }

        private sealed class InvalidGraphModel : IGraphModel
        {
            public Type[] GetTypes(string label) => throw new InvalidOperationException();

            public IGraphElementModel VerticesModel { get => GraphElementModel.Invalid; }
            public IGraphElementModel EdgesModel { get => GraphElementModel.Invalid; }
        }

        private sealed class AssemblyGraphModel : IGraphModel
        {
            private sealed class AssemblyGraphElementModel : IGraphElementModel
            {
                private readonly Type _baseType;
                private readonly ConcurrentDictionary<Type, string[]> _derivedLabels = new ConcurrentDictionary<Type, string[]>();

                public AssemblyGraphElementModel(Type baseType, string idPropertyName, IEnumerable<Assembly> assemblies, ILogger logger)
                {
                    _baseType = baseType;

                    Labels = assemblies
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

                    IdPropertyName = idPropertyName;
                }

                public Option<string> TryGetConstructiveVertexLabel(Type elementType)
                {
                    return Labels
                        .TryGetValue(elementType)
                        .Map(x => x.FirstOrDefault())
                        .IfNone(() => elementType.BaseType != null
                            ? TryGetConstructiveVertexLabel(elementType.BaseType)
                            : default);
                }

                public Option<string> TryGetConstructiveLabel(Type elementType)
                {
                    return Labels
                        .TryGetValue(elementType)
                        .Map(x => x.FirstOrDefault())
                        .IfNone(() => elementType.BaseType != null
                            ? TryGetConstructiveVertexLabel(elementType.BaseType)
                            : default);
                }

                public Option<string[]> TryGetFilterLabels(Type elementType)
                {
                    return elementType.IsAssignableFrom(_baseType)
                        ? default
                        : _derivedLabels.GetOrAdd(
                            elementType,
                            closureType => Labels
                                .Where(kvp => !kvp.Key.GetTypeInfo().IsAbstract && closureType.IsAssignableFrom(kvp.Key))
                                .Select(kvp => kvp.Value[0])
                                .OrderBy(x => x)
                                .ToArray());
                }

                public Option<string> IdPropertyName { get; }
                public IDictionary<Type, string[]> Labels { get; }
            }

            private readonly IDictionary<string, Type[]> _types;
            private readonly AssemblyGraphElementModel _edgeModel;
            private readonly AssemblyGraphElementModel _vertexModel;

            public AssemblyGraphModel(Type vertexBaseType, Type edgeBaseType, string vertexIdPropertyName, string edgeIdPropertyName, IEnumerable<Assembly> assemblies, ILogger logger)
            {
                if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                    throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

                if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                    throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

                _vertexModel = new AssemblyGraphElementModel(vertexBaseType, vertexIdPropertyName, assemblies, logger);
                _edgeModel = new AssemblyGraphElementModel(edgeBaseType, edgeIdPropertyName, assemblies, logger);

                _types =_vertexModel.Labels
                    .Concat(_edgeModel.Labels)
                    .GroupBy(x => x.Value[0])
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(x => x.Key)
                            .ToArray());
            }

            public Type[] GetTypes(string label)
            {
                return _types
                    .TryGetValue(label)
                    .IfNone(Array.Empty<Type>());
            }

            public IGraphElementModel EdgesModel => _edgeModel;
            public IGraphElementModel VerticesModel => _vertexModel;
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
            return new AssemblyGraphModel(vertexBaseType, edgeBaseType, vertexIdPropertyName, edgeIdPropertyName, assemblies, logger);
        }

        internal static object GetIdentifier(this IGraphModel model, GraphElementType elementType, string name)
        {
            return elementType == GraphElementType.Vertex && name == model.VerticesModel.IdPropertyName
                || elementType == GraphElementType.Edge && name == model.EdgesModel.IdPropertyName
                ? (object)T.Id
                : name;
        }
    }
}
