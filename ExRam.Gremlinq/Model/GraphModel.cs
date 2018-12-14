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

            public Option<string> EdgeIdPropertyName { get; }
            public Option<string> VertexIdPropertyName { get; }
        }

        private sealed class AssemblyGraphModelImpl : IGraphModel
        {
            private readonly IImmutableDictionary<Type, string> _edgeLabels;
            private readonly IImmutableDictionary<Type, string> _vertexLabels;

            public AssemblyGraphModelImpl(Assembly assembly, Type vertexBaseType, Type edgeBaseType, string idPropertyName, string edgeIdPropertyName)
            {
                if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                    throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

                if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                    throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

                _vertexLabels = assembly
                    .DefinedTypes
                    .Prepend(vertexBaseType)
                    .Where(vertexBaseType.IsAssignableFrom)
                    .Select(typeInfo => typeInfo)
                    .ToImmutableDictionary(
                        type => type,
                        type => type.Name);

                _edgeLabels = assembly
                    .DefinedTypes
                    .Prepend(edgeBaseType)
                    .Where(edgeBaseType.IsAssignableFrom)
                    .Select(typeInfo => typeInfo)
                    .ToImmutableDictionary(
                        type => type,
                        type => type.Name);

                VertexIdPropertyName = idPropertyName;
                EdgeIdPropertyName = edgeIdPropertyName;
            }

            public Option<string> TryGetLabel(Type elementType)
            {
                return _vertexLabels
                    .TryGetValue(elementType)
                    .Match(
                        _ => _,
                        () => _edgeLabels
                            .TryGetValue(elementType));
            }

            public Option<Type> TryGetType(string label)
            {
                return _vertexLabels
                    .Concat(_edgeLabels)
                    .TryGetElementTypeOfLabel(label);
            }

            public string[] TryGetDerivedLabels(Type elementType)
            {
                return GetDerivedTypes(elementType, true)
                    .Select(closureType => TryGetLabel(closureType)
                        .IfNone(() => throw new InvalidOperationException()))
                    .OrderBy(x => x)
                    .ToArray();
            }

            private IEnumerable<Type> GetDerivedTypes(Type type, bool includeType)
            {
                return _vertexLabels.Keys
                    .Concat(_edgeLabels.Keys)
                    .Where(kvp => !kvp.GetTypeInfo().IsAbstract && (includeType || kvp != type) && type.IsAssignableFrom(kvp));
            }

            public Option<string> VertexIdPropertyName { get; }
            public Option<string> EdgeIdPropertyName { get; }
        }

        public static readonly IGraphModel Empty = new EmptyGraphModel();

        public static IGraphModel FromAssembly<TVertex, TEdge>(Assembly assembly, string idPropertyName = "Id", string edgeIdPropertyName = "Id")
        {
            return FromAssembly(assembly, typeof(TVertex), typeof(TEdge), idPropertyName, edgeIdPropertyName);
        }

        public static IGraphModel FromAssembly(Assembly assembly, Type vertexBaseType, Type edgeBaseType, string vertexIdPropertyName = "Id", string edgeIdPropertyName = "Id")
        {
            return new AssemblyGraphModelImpl(assembly, vertexBaseType, edgeBaseType, vertexIdPropertyName, edgeIdPropertyName);
        }

        internal static Option<Type> TryGetElementTypeOfLabel(this IEnumerable<KeyValuePair<Type, string>> elementInfos, string label)
        {
            return elementInfos
                .Where(elementInfo => elementInfo.Value.Equals(label, StringComparison.OrdinalIgnoreCase))
                .Select(elementInfo => elementInfo.Key)
                .FirstOrDefault();
        }
    }
}
