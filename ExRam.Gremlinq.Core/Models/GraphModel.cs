using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.Extensions;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class GraphModel
    {
        private abstract class GraphModelWrapper : IGraphModel
        {
            protected GraphModelWrapper(IGraphModel baseModel)
            {
                BaseModel = baseModel;
            }

            public virtual Type[] GetTypes(string label) => BaseModel.GetTypes(label);

            public virtual IGraphElementModel VerticesModel { get => BaseModel.VerticesModel; }
            public virtual IGraphElementModel EdgesModel { get => BaseModel.EdgesModel; }

            public virtual object GetIdentifier(Expression expression) => BaseModel.GetIdentifier(expression);

            public virtual PropertyMetadata GetPropertyMetadata(PropertyInfo property) => BaseModel.GetPropertyMetadata(property);

            public virtual IGraphModel ConfigureElement<TElement>(Action<IElementConfigurator<TElement>> action)
            {
                return new ConfiguredGraphModel(this, ImmutableDictionary<PropertyInfo, PropertyMetadata>.Empty)
                    .ConfigureElement(action);
            }

            protected IGraphModel BaseModel { get; }
        }

        private sealed class ConfiguredGraphModel : GraphModelWrapper
        {
            private readonly ImmutableDictionary<PropertyInfo, PropertyMetadata> _propertyMetadataDictionary;

            public ConfiguredGraphModel(IGraphModel model, ImmutableDictionary<PropertyInfo, PropertyMetadata> propertyMetadataDictionary) : base(model)
            {
                _propertyMetadataDictionary = propertyMetadataDictionary;
            }

            public override IGraphModel ConfigureElement<TElement>(Action<IElementConfigurator<TElement>> action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                var builder = new ElementConfigurator<TElement>();

                action(builder);

                var newDict = _propertyMetadataDictionary;

                foreach (var updateSemanticsKvp in builder.UpdateSemantics)
                {
                    newDict = newDict.SetItem(updateSemanticsKvp.Key, new PropertyMetadata(updateSemanticsKvp.Value));
                }

                return new ConfiguredGraphModel(BaseModel, newDict);
            }

            public override PropertyMetadata GetPropertyMetadata(PropertyInfo property)
            {
                return _propertyMetadataDictionary
                    .TryGetValue(property)
                    .IfNone(() => base.GetPropertyMetadata(property));
            }
        }

        private sealed class CamelcaseLabelGraphModel : GraphModelWrapper
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

            public CamelcaseLabelGraphModel(IGraphModel model) : base(model)
            {
                EdgesModel = new CamelcaseGraphElementModel(model.EdgesModel);
                VerticesModel = new CamelcaseGraphElementModel(model.VerticesModel);
            }

            public override IGraphElementModel EdgesModel { get; }
            public override IGraphElementModel VerticesModel { get; }
        }

        private sealed class CamelcasePropertiesGraphModel : GraphModelWrapper
        {
            public CamelcasePropertiesGraphModel(IGraphModel model) : base(model)
            {
            }

            public override object GetIdentifier(Expression expression)
            {
                var retVal = base.GetIdentifier(expression);

                return retVal is string identifier ? identifier.ToCamelCase() : retVal;
            }
        }

        private sealed class LowercaseGraphModel : GraphModelWrapper
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

            public LowercaseGraphModel(IGraphModel model) : base(model)
            {
                EdgesModel = new LowercaseGraphElementModel(model.EdgesModel);
                VerticesModel = new LowercaseGraphElementModel(model.VerticesModel);
            }

            public override IGraphElementModel EdgesModel { get; }
            public override IGraphElementModel VerticesModel { get; }
        }

        private sealed class RelaxedGraphModel : GraphModelWrapper
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
                        .IfNone(new[] { elementType.Name });
                }
            }

            public RelaxedGraphModel(IGraphModel baseGraphModel) : base(baseGraphModel)
            {
                VerticesModel = new RelaxedGraphElementModel(baseGraphModel.VerticesModel);
                EdgesModel = new RelaxedGraphElementModel(baseGraphModel.EdgesModel);
            }

            public override IGraphElementModel VerticesModel { get; }

            public override IGraphElementModel EdgesModel { get; }
        }

        private sealed class EmptyGraphModel : IGraphModel
        {
            public Type[] GetTypes(string label) => Array.Empty<Type>();

            public object GetIdentifier(Expression expression)
            {
                if (expression is MemberExpression memberExpression)
                {
                    var memberName = memberExpression.Member.Name;

                    if (string.Equals(memberName, "id", StringComparison.OrdinalIgnoreCase))
                        return T.Id;

                    if (string.Equals(memberName, "label", StringComparison.OrdinalIgnoreCase))
                        return T.Label;

                    return memberName;
                }

                throw new ExpressionNotSupportedException(expression);
            }

            public PropertyMetadata GetPropertyMetadata(PropertyInfo property)
            {
                return new PropertyMetadata(IgnoreDirective.Never);
            }

            public IGraphModel ConfigureElement<TElement>(Action<IElementConfigurator<TElement>> action)
            {
                throw new InvalidOperationException();
            }

            public IGraphElementModel VerticesModel { get => GraphElementModel.Empty; }
            public IGraphElementModel EdgesModel { get => GraphElementModel.Empty; }
        }

        private sealed class InvalidGraphModel : IGraphModel
        {
            private const string ErrorMessage = "'{0}' must not be called on GraphModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public Type[] GetTypes(string label) => throw new InvalidOperationException(string.Format(ErrorMessage, nameof(GetTypes)));

            public object GetIdentifier(Expression expression)
            {
                throw new ExpressionNotSupportedException(expression);
            }

            public PropertyMetadata GetPropertyMetadata(PropertyInfo property)
            {
                throw new InvalidOperationException();
            }

            public IGraphModel ConfigureElement<TElement>(Action<IElementConfigurator<TElement>> action)
            {
                throw new InvalidOperationException();
            }

            public IGraphElementModel VerticesModel { get => GraphElementModel.Invalid; }
            public IGraphElementModel EdgesModel { get => GraphElementModel.Invalid; }
        }

        private sealed class AssemblyGraphModel : GraphModelWrapper
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

            public AssemblyGraphModel(Type vertexBaseType, Type edgeBaseType, IEnumerable<Assembly> assemblies, ILogger logger) : base(Empty)
            {
                if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                    throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

                if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                    throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

                var assemblyArray = assemblies.ToArray();

                _verticesModel = new AssemblyGraphElementModel(vertexBaseType, assemblyArray, logger);
                _edgesModel = new AssemblyGraphElementModel(edgeBaseType, assemblyArray, logger);

                _types = _verticesModel.Labels
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

        public static IGraphModel WithCamelcaseProperties(this IGraphModel model)
        {
            return new CamelcasePropertiesGraphModel(model);
        }
    }
}
