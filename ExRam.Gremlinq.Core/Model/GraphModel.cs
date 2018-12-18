using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.GraphElements;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public static class GraphModel
    {
        private sealed class EmptyGraphModel : IGraphModel
        {
            public Option<string> TryGetLabel(Type elementType)
            {
                return default;
            }

            public Option<Type> TryGetType(string label)
            {
                return default;
            }

            public string[] TryGetDerivedLabels(Type elementType)
            {
                return Array.Empty<string>();
            }

            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public Option<string> EdgeIdPropertyName { get; }

            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public Option<string> VertexIdPropertyName { get; }
        }

        private sealed class AssemblyGraphModelImpl : IGraphModel
        {
            private readonly IDictionary<string, Type> _types;
            private readonly IDictionary<Type, string> _labels;
            private readonly ConcurrentDictionary<Type, string[]> _derivedLabels = new ConcurrentDictionary<Type, string[]>();

            public AssemblyGraphModelImpl(Type vertexBaseType, Type edgeBaseType, string idPropertyName, string edgeIdPropertyName, IEnumerable<Assembly> assemblies)
            {
                if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                    throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

                if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                    throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

                _labels = assemblies
                    .SelectMany(assembly => assembly
                        .DefinedTypes
                        .Where(type => type != vertexBaseType
                                    && type != edgeBaseType
                                    && (vertexBaseType.IsAssignableFrom(type) || edgeBaseType.IsAssignableFrom(type)))
                        .Select(typeInfo => typeInfo))
                    .Prepend(vertexBaseType)
                    .Prepend(edgeBaseType)
                    .Where(x => !x.IsInterface)
                    .ToDictionary(
                        type => type,
                        type => type.Name);

                _types = _labels
                    .ToDictionary(
                        kvp => kvp.Value,
                        kvp => kvp.Key);

                VertexIdPropertyName = idPropertyName;
                EdgeIdPropertyName = edgeIdPropertyName;
            }

            public Option<string> TryGetLabel(Type elementType)
            {
                return _labels
                    .TryGetValue(elementType);
            }

            public Option<Type> TryGetType(string label)
            {
                return _types
                    .TryGetValue(label);
            }

            public string[] TryGetDerivedLabels(Type elementType)
            {
                return _derivedLabels.GetOrAdd(
                    elementType,
                    closureType1 => _labels
                        .Where(kvp => !kvp.Key.GetTypeInfo().IsAbstract && closureType1.IsAssignableFrom(kvp.Key))
                        .Select(kvp => kvp.Value)
                        .OrderBy(x => x)
                        .ToArray());
            }

            public Option<string> VertexIdPropertyName { get; }
            public Option<string> EdgeIdPropertyName { get; }
        }

        public static readonly IGraphModel Empty = new EmptyGraphModel();

        public static IGraphModel Dynamic()
        {
            return FromAssemblies<IVertex, IEdge>(x => x.Id, x => x.Id, AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IGraphModel FromExecutingAssembly()
        {
            return FromAssemblies<IVertex, IEdge>(x => x.Id, x => x.Id, Assembly.GetCallingAssembly());
        }

        public static IGraphModel FromExecutingAssembly<TVertex, TEdge>(Expression<Func<TVertex, object>> vertexId, Expression<Func<TVertex, object>> edgeId)
        {
            return FromAssemblies<TVertex, TEdge>(vertexId, edgeId, Assembly.GetCallingAssembly());
        }

        public static IGraphModel FromAssemblies<TVertex, TEdge>(Expression<Func<TVertex, object>> vertexId, Expression<Func<TVertex, object>> edgeId, params Assembly[] assemblies)
        {
            return FromAssemblies(typeof(TVertex), typeof(TEdge), ((MemberExpression)vertexId.Body.StripConvert()).Member.Name, ((MemberExpression)edgeId.Body.StripConvert()).Member.Name, assemblies);
        }

        public static IGraphModel FromAssemblies(Type vertexBaseType, Type edgeBaseType, string vertexIdPropertyName = "Id", string edgeIdPropertyName = "Id", params Assembly[] assemblies)
        {
            return new AssemblyGraphModelImpl(vertexBaseType, edgeBaseType, vertexIdPropertyName, edgeIdPropertyName, assemblies);
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
