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
        private enum GraphElementType
        {
            None,
            Vertex,
            Edge,
            VertexProperty
        }

        private sealed class EmptyGraphModel : IGraphModel
        {
            public Type[] GetTypes(string label) => Array.Empty<Type>();

            public IGraphElementModel VerticesModel { get => GraphElementModel.Empty; }
            public IGraphElementModel EdgesModel { get => GraphElementModel.Empty; }
        }

        private sealed class InvalidGraphModel : IGraphModel
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public Type[] GetTypes(string label) => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(GetTypes)));

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
            private readonly AssemblyGraphElementModel _edgesModel;
            private readonly AssemblyGraphElementModel _verticesModel;

            public AssemblyGraphModel(Type vertexBaseType, Type edgeBaseType, string vertexIdPropertyName, string edgeIdPropertyName, IEnumerable<Assembly> assemblies, ILogger logger)
            {
                if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                    throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

                if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                    throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

                var assemblyArray = assemblies.ToArray();

                _verticesModel = new AssemblyGraphElementModel(vertexBaseType, vertexIdPropertyName, assemblyArray, logger);
                _edgesModel = new AssemblyGraphElementModel(edgeBaseType, edgeIdPropertyName, assemblyArray, logger);

                _types =_verticesModel.Labels
                    .Concat(_edgesModel.Labels)
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

            public IGraphElementModel EdgesModel => _edgesModel;
            public IGraphElementModel VerticesModel => _verticesModel;
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

        internal static object GetIdentifier(this IGraphModel model, Expression expression)
        {
            switch (expression)
            {
                case MemberExpression leftMemberExpression:
                {
                    return model.GetIdentifier(leftMemberExpression.Expression.Type, leftMemberExpression.Member.Name);
                }
                case ParameterExpression leftParameterExpression:
                {
                    return model.GetIdentifier(leftParameterExpression.Type, leftParameterExpression.Name);
                }
                default:
                    throw new ExpressionNotSupportedException(expression);
            }
        }

        internal static object GetIdentifier(this IGraphModel model, Type elementType, string memberName)
        {
            var graphElementType = GraphElementType.None;

            if (elementType == typeof(IVertex) || model.VerticesModel.TryGetConstructiveLabel(elementType).IsSome)
                graphElementType = GraphElementType.Vertex;

            if (elementType == typeof(IEdge) || model.EdgesModel.TryGetConstructiveLabel(elementType).IsSome)
                graphElementType = GraphElementType.Edge;

            if (elementType.IsGenericType && (elementType.GetGenericTypeDefinition() == typeof(VertexProperty<>) || elementType.GetGenericTypeDefinition() == typeof(VertexProperty<,>)))
                graphElementType = GraphElementType.VertexProperty;

            if (graphElementType == GraphElementType.Vertex && memberName == model.VerticesModel.IdPropertyName || graphElementType == GraphElementType.Edge && memberName == model.EdgesModel.IdPropertyName || graphElementType == GraphElementType.VertexProperty && memberName == nameof(VertexProperty<object>.Id))
                return T.Id;

            if (graphElementType == GraphElementType.VertexProperty && memberName == nameof(VertexProperty<object>.Label))
                return T.Label;

            return memberName;
        }
    }
}
