using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Extensions;
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

        private abstract class GraphModelBase : IGraphModel
        {
            public abstract IGraphElementModel VerticesModel { get; }
            public abstract IGraphElementModel EdgesModel { get; }

            public abstract Type[] GetTypes(string label);

            public virtual object GetIdentifier(Expression expression)
            {
                switch (expression)
                {
                    case MemberExpression leftMemberExpression:
                        {
                            return GetIdentifier(leftMemberExpression.Expression.Type, leftMemberExpression.Member.Name);
                        }
                    case ParameterExpression leftParameterExpression:
                        {
                            return GetIdentifier(leftParameterExpression.Type, leftParameterExpression.Name);
                        }
                    default:
                        throw new ExpressionNotSupportedException(expression);
                }
            }

            public virtual object GetIdentifier(Type elementType, string memberName)
            {
                if (string.Equals(memberName, "id", StringComparison.OrdinalIgnoreCase) || string.Equals(memberName, "label", StringComparison.OrdinalIgnoreCase))
                {
                    var graphElementType = ElementTypes
                        .GetOrCreateValue(this)
                        .GetOrAdd(
                            elementType,
                            closureElementType =>
                            {
                                if (elementType == typeof(IVertex) || VerticesModel.TryGetConstructiveLabel(elementType).IsSome)
                                    return GraphElementType.Vertex;

                                if (elementType == typeof(IEdge) || EdgesModel.TryGetConstructiveLabel(elementType).IsSome)
                                    return GraphElementType.Edge;

                                return typeof(IVertexProperty).IsAssignableFrom(elementType)
                                    ? GraphElementType.VertexProperty
                                    : GraphElementType.None;
                            });

                    if (graphElementType != GraphElementType.None)
                    {
                        if (string.Equals(memberName, "id", StringComparison.OrdinalIgnoreCase))
                            return T.Id;

                        if (string.Equals(memberName, "label", StringComparison.OrdinalIgnoreCase))
                            return T.Label;
                    }
                }

                return memberName;
            }
        }

        private sealed class CamelcaseLabelGraphModel : GraphModelBase
        {
            private sealed class CamelcaseGraphElementModel : IGraphElementModel
            {
                private readonly IGraphElementModel _baseModel;

                public CamelcaseGraphElementModel(IGraphElementModel baseModel)
                {
                    _baseModel = baseModel;
                }

                public Option<string> TryGetConstructiveLabel(Type elementType) => _baseModel.TryGetConstructiveLabel(elementType).Map(x => x.ToCamelCase());

                public Option<string[]> TryGetFilterLabels(Type elementType) => _baseModel.TryGetFilterLabels(elementType).Map(x => x.Select(y => y.ToCamelCase()).ToArray());
            }

            private readonly IGraphModel _model;

            public CamelcaseLabelGraphModel(IGraphModel model)
            {
                _model = model;
                EdgesModel = new CamelcaseGraphElementModel(model.EdgesModel);
                VerticesModel = new CamelcaseGraphElementModel(model.VerticesModel);
            }

            public override IGraphElementModel EdgesModel { get; }
            public override IGraphElementModel VerticesModel { get; }
            public override Type[] GetTypes(string label) => _model.GetTypes(label);
        }

        private sealed class CamelcaseIdentifiersGraphModel : GraphModelBase
        {
            private readonly IGraphModel _model;

            public CamelcaseIdentifiersGraphModel(IGraphModel model)
            {
                _model = model;
            }

            public override object GetIdentifier(Type elementType, string memberName)
            {
                var retVal = base.GetIdentifier(elementType, memberName);

                return retVal is string identifier ? identifier.ToCamelCase() : retVal;
            }

            public override IGraphElementModel EdgesModel => _model.EdgesModel;
            public override IGraphElementModel VerticesModel => _model.VerticesModel;
            public override Type[] GetTypes(string label) => _model.GetTypes(label);
        }

        private sealed class LowercaseGraphModel : GraphModelBase
        {
            private sealed class LowercaseGraphElementModel : IGraphElementModel
            {
                private readonly IGraphElementModel _baseModel;

                public LowercaseGraphElementModel(IGraphElementModel baseModel)
                {
                    _baseModel = baseModel;
                }

                public Option<string> TryGetConstructiveLabel(Type elementType) => _baseModel.TryGetConstructiveLabel(elementType).Map(x => x.ToLower());

                public Option<string[]> TryGetFilterLabels(Type elementType) => _baseModel.TryGetFilterLabels(elementType).Map(x => x.Select(y => y.ToLower()).ToArray());
            }

            private readonly IGraphModel _model;

            public LowercaseGraphModel(IGraphModel model)
            {
                _model = model;
                EdgesModel = new LowercaseGraphElementModel(model.EdgesModel);
                VerticesModel = new LowercaseGraphElementModel(model.VerticesModel);
            }

            public override IGraphElementModel EdgesModel { get; }
            public override IGraphElementModel VerticesModel { get; }
            public override Type[] GetTypes(string label) => _model.GetTypes(label);
        }

        private sealed class RelaxedGraphModel : GraphModelBase
        {
            private sealed class RelaxedGraphElementModel : IGraphElementModel
            {
                private readonly IGraphElementModel _baseGraphElementModel;

                public RelaxedGraphElementModel(IGraphElementModel baseGraphElementModel)
                {
                    _baseGraphElementModel = baseGraphElementModel;
                }

                public Option<string> TryGetConstructiveLabel(Type elementType)
                {
                    return _baseGraphElementModel
                        .TryGetConstructiveLabel(elementType)
                        .IfNone(elementType.Name);
                }

                public Option<string[]> TryGetFilterLabels(Type elementType)
                {
                    return _baseGraphElementModel
                        .TryGetFilterLabels(elementType)
                        .IfNone(new[] {elementType.Name});
                }
            }

            private readonly IGraphModel _baseGraphModel;

            public RelaxedGraphModel(IGraphModel baseGraphModel)
            {
                _baseGraphModel = baseGraphModel;

                VerticesModel = new RelaxedGraphElementModel(baseGraphModel.VerticesModel);
                EdgesModel = new RelaxedGraphElementModel(baseGraphModel.EdgesModel);
            }

            public override Type[] GetTypes(string label) => _baseGraphModel.GetTypes(label);

            public override IGraphElementModel VerticesModel { get; }

            public override IGraphElementModel EdgesModel { get; }
        }

        private sealed class EmptyGraphModel : GraphModelBase
        {
            public override Type[] GetTypes(string label) => Array.Empty<Type>();

            public override IGraphElementModel VerticesModel { get => GraphElementModel.Empty; }
            public override IGraphElementModel EdgesModel { get => GraphElementModel.Empty; }
        }

        private sealed class InvalidGraphModel : GraphModelBase
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public override Type[] GetTypes(string label) => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(GetTypes)));

            public override IGraphElementModel VerticesModel { get => GraphElementModel.Invalid; }
            public override IGraphElementModel EdgesModel { get => GraphElementModel.Invalid; }
        }

        private sealed class AssemblyGraphModel : GraphModelBase
        {
            private sealed class AssemblyGraphElementModel : IGraphElementModel
            {
                private readonly Type _baseType;
                private readonly ConcurrentDictionary<Type, Option<string[]>> _derivedLabels = new ConcurrentDictionary<Type, Option<string[]>>();

                public AssemblyGraphElementModel(Type baseType, IEnumerable<Assembly> assemblies, ILogger logger)
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
                }

                public Option<string> TryGetConstructiveLabel(Type elementType)
                {
                    return elementType
                        .GetTypeHierarchy()
                        .Where(type => !type.IsAbstract)
                        .SelectMany(type => Labels
                            .TryGetValue(type)
                            .Map(x => x.FirstOrDefault()))
                        .FirstOrDefault();
                }

                public Option<string[]> TryGetFilterLabels(Type elementType)
                {
                    return elementType.IsAssignableFrom(_baseType)
                        ? Array.Empty<string>()
                        : _derivedLabels.GetOrAdd(
                            elementType,
                            closureType =>
                            {
                                var labels = Labels
                                    .Where(kvp => !kvp.Key.GetTypeInfo().IsAbstract && closureType.IsAssignableFrom(kvp.Key))
                                    .Select(kvp => kvp.Value[0])
                                    .OrderBy(x => x)
                                    .ToArray();

                                return labels.Length == 0
                                    ? default(Option<string[]>)
                                    : labels;
                            });
                }

                public IDictionary<Type, string[]> Labels { get; }
            }

            private readonly IDictionary<string, Type[]> _types;
            private readonly AssemblyGraphElementModel _edgesModel;
            private readonly AssemblyGraphElementModel _verticesModel;

            public AssemblyGraphModel(Type vertexBaseType, Type edgeBaseType, IEnumerable<Assembly> assemblies, ILogger logger)
            {
                if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                    throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

                if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                    throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

                var assemblyArray = assemblies.ToArray();

                _verticesModel = new AssemblyGraphElementModel(vertexBaseType, assemblyArray, logger);
                _edgesModel = new AssemblyGraphElementModel(edgeBaseType, assemblyArray, logger);

                _types =_verticesModel.Labels
                    .Concat(_edgesModel.Labels)
                    .GroupBy(x => x.Value[0])
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(x => x.Key)
                            .ToArray(),
                        StringComparer.OrdinalIgnoreCase);
            }

            public override Type[] GetTypes(string label)
            {
                return _types
                    .TryGetValue(label)
                    .IfNone(Array.Empty<Type>());
            }

            public override IGraphElementModel EdgesModel => _edgesModel;
            public override IGraphElementModel VerticesModel => _verticesModel;
        }

        public static readonly IGraphModel Empty = new EmptyGraphModel();
        public static readonly IGraphModel Invalid = new InvalidGraphModel();

        private static readonly ConditionalWeakTable<IGraphModel, ConcurrentDictionary<Type, GraphElementType>> ElementTypes = new ConditionalWeakTable<IGraphModel, ConcurrentDictionary<Type, GraphElementType>>();

        public static IGraphModel Dynamic(ILogger logger = null)
        {
            return FromAssemblies<IVertex, IEdge>(logger, AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IGraphModel FromBaseTypes<TVertex, TEdge>(ILogger logger = null)
        {
            return FromAssemblies<TVertex, TEdge>(logger, typeof(TVertex).Assembly, typeof(TEdge).Assembly);
        }

        public static IGraphModel FromExecutingAssembly(ILogger logger = null)
        {
            return FromAssemblies<IVertex, IEdge>(logger, Assembly.GetCallingAssembly());
        }

        public static IGraphModel FromExecutingAssembly<TVertex, TEdge>(ILogger logger = null)
        {
            return FromAssemblies<TVertex, TEdge>(logger, Assembly.GetCallingAssembly());
        }

        public static IGraphModel FromAssemblies<TVertex, TEdge>(ILogger logger = null, params Assembly[] assemblies)
        {
            return FromAssemblies(typeof(TVertex), typeof(TEdge), logger, assemblies);
        }

        public static IGraphModel FromAssemblies(Type vertexBaseType, Type edgeBaseType, ILogger logger = null, params Assembly[] assemblies)
        {
            return new AssemblyGraphModel(vertexBaseType, edgeBaseType, assemblies, logger);
        }

        public static IGraphModel Relax(this IGraphModel model)
        {
            return new RelaxedGraphModel(model);
        }

        public static IGraphModel WithLowercaseLabels(this IGraphModel model)
        {
            return new LowercaseGraphModel(model);
        }

        public static IGraphModel WithCamelcaseLabels(this IGraphModel model)
        {
            return new CamelcaseLabelGraphModel(model);
        }

        public static IGraphModel WithCamelcaseIdentifiers(this IGraphModel model)
        {
            return new CamelcaseIdentifiersGraphModel(model);
        }

    }
}
