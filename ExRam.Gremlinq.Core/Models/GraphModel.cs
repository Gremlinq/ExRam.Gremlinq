using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.Extensions;
using ExRam.Gremlinq.Core.GraphElements;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class GraphElementPropertiesModel
    {
        private sealed class GraphElementPropertiesModelImpl : IGraphElementPropertiesModel
        {
            private readonly IGraphElementPropertiesModel _baseModel;

            public GraphElementPropertiesModelImpl(
                IGraphElementPropertiesModel baseModel,
                IImmutableDictionary<PropertyInfo, PropertyMetadata> metaData)
            {
                _baseModel = baseModel;
                MetaData = metaData;
            }

            public IImmutableDictionary<PropertyInfo, PropertyMetadata> MetaData { get; }

            public object GetIdentifier(Expression expression) => _baseModel.GetIdentifier(expression);
        }

        private sealed class CamelCaseGraphElementPropertiesModel : IGraphElementPropertiesModel
        {
            private readonly IGraphElementPropertiesModel _model;

            public CamelCaseGraphElementPropertiesModel(IGraphElementPropertiesModel model)
            {
                _model = model;
            }

            public object GetIdentifier(Expression expression)
            {
                var retVal = _model.GetIdentifier(expression);

                return retVal is string identifier ? identifier.ToCamelCase() : retVal;
            }

            public IImmutableDictionary<PropertyInfo, PropertyMetadata> MetaData => _model.MetaData;
        }

        private sealed class LowerCaseGraphElementPropertiesModel : IGraphElementPropertiesModel
        {
            private readonly IGraphElementPropertiesModel _model;

            public LowerCaseGraphElementPropertiesModel(IGraphElementPropertiesModel model)
            {
                _model = model;
            }

            public object GetIdentifier(Expression expression)
            {
                var retVal = _model.GetIdentifier(expression);

                return retVal is string identifier ? identifier.ToCamelCase() : retVal;
            }

            public IImmutableDictionary<PropertyInfo, PropertyMetadata> MetaData => _model.MetaData;
        }

        public static IGraphElementPropertiesModel WithCamelCaseProperties(this IGraphElementPropertiesModel model)
        {
            return new CamelCaseGraphElementPropertiesModel(model);
        }

        public static IGraphElementPropertiesModel WithLowerCaseProperties(this IGraphElementPropertiesModel model)
        {
            return new LowerCaseGraphElementPropertiesModel(model);
        }

        public static IGraphElementPropertiesModel ConfigureElement<TElement>(this IGraphElementPropertiesModel model, Action<IElementConfigurator<TElement>> action)
        {
            var builder = new ElementConfigurator<TElement>();

            action(builder);

            var dict = model.MetaData;
            
            foreach (var updateSemanticsKvp in builder.UpdateSemantics)
            {
                dict = dict.SetItem(updateSemanticsKvp.Key, new PropertyMetadata(updateSemanticsKvp.Value));
            }

            return new GraphElementPropertiesModelImpl(model, dict);
        }
    }

    public static class GraphModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            public GraphModelImpl(IGraphElementModel verticesModel, IGraphElementModel edgesModel, IGraphElementPropertiesModel propertiesModel)
            {
                VerticesModel = verticesModel;
                EdgesModel = edgesModel;
                PropertiesModel = propertiesModel;
            }

            public IGraphElementModel VerticesModel { get; }

            public IGraphElementModel EdgesModel { get; }

            public IGraphElementPropertiesModel PropertiesModel { get; }
        }

        private sealed class EmptyGraphModel : IGraphModel
        {
            private sealed class EmptyGraphElementPropertiesModel : IGraphElementPropertiesModel
            {
                public IImmutableDictionary<PropertyInfo, PropertyMetadata> MetaData => ImmutableDictionary<PropertyInfo, PropertyMetadata>.Empty;

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
            }

            public IGraphElementModel VerticesModel { get => GraphElementModel.Empty; }
            public IGraphElementModel EdgesModel { get => GraphElementModel.Empty; }

            public IGraphElementPropertiesModel PropertiesModel { get; } = new EmptyGraphElementPropertiesModel();
        }

        private sealed class InvalidGraphModel : IGraphModel
        {
            private sealed class InvalidGraphElementPropertiesModel : IGraphElementPropertiesModel
            {
                public IImmutableDictionary<PropertyInfo, PropertyMetadata> MetaData => throw new InvalidOperationException();

                public object GetIdentifier(Expression expression)
                {
                    throw new InvalidOperationException();
                }
            }

            private const string ErrorMessage = "'{0}' must not be called on GraphModel.Invalid. If you are getting this exception while executing a query, set a proper GraphModel on the GremlinQuerySource (e.g. by calling 'g.WithModel(...)').";

            public IGraphElementModel VerticesModel { get => GraphElementModel.Invalid; }
            public IGraphElementModel EdgesModel { get => GraphElementModel.Invalid; }
            public IGraphElementPropertiesModel PropertiesModel { get => throw new InvalidOperationException(); }
        }

        private sealed class AssemblyGraphModel : IGraphModel
        {
            private sealed class AssemblyGraphElementModel : IGraphElementModel
            {
                public AssemblyGraphElementModel(Type baseType, IEnumerable<Assembly> assemblies, ILogger logger)
                {
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
                        .Where(type => !type.IsAbstract)
                        .ToImmutableDictionary(
                            type => type,
                            type => type.Name);
                }

                public IImmutableDictionary<Type, string> Labels { get; }
            }

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
            }

            public IGraphElementModel EdgesModel => _edgesModel;
            public IGraphElementModel VerticesModel => _verticesModel;
            public IGraphElementPropertiesModel PropertiesModel => Empty.PropertiesModel;
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

        public static IGraphModel WithVerticesModel(this IGraphModel model, Func<IGraphElementModel, IGraphElementModel> transformation)
        {
            return new GraphModelImpl(
                transformation(model.VerticesModel),
                model.EdgesModel,
                model.PropertiesModel);
        }

        public static IGraphModel WithEdgesModel(this IGraphModel model, Func<IGraphElementModel, IGraphElementModel> transformation)
        {
            return new GraphModelImpl(
                model.VerticesModel,
                transformation(model.EdgesModel),
                model.PropertiesModel);
        }

        public static IGraphModel WithPropertiesModel(this IGraphModel model, Func<IGraphElementPropertiesModel, IGraphElementPropertiesModel> transformation)
        {
            return new GraphModelImpl(
                model.VerticesModel,
                model.EdgesModel,
                transformation(model.PropertiesModel));
        }

        public static IGraphModel WithLowerCaseLabels(this IGraphModel model)
        {
            return model
                .WithVerticesModel(_ => _.WithLowerCaseLabels())
                .WithEdgesModel(_ => _.WithLowerCaseLabels());
        }

        public static IGraphModel WithLowerCaseProperties(this IGraphModel model)
        {
            return model
                .WithPropertiesModel(_ => _.WithLowerCaseProperties());
        }

        public static IGraphModel WithCamelCaseLabels(this IGraphModel model)
        {
            return model
                .WithVerticesModel(_ => _.WithCamelCaseLabels())
                .WithEdgesModel(_ => _.WithCamelCaseLabels());
        }

        public static IGraphModel WithCamelCaseProperties(this IGraphModel model)
        {
            return model
                .WithPropertiesModel(_ => _.WithCamelCaseProperties());
        }

        public static IGraphModel ConfigureElement<TElement>(this IGraphModel model, Action<IElementConfigurator<TElement>> action)
        {
            return model
                .WithPropertiesModel(_ => _.ConfigureElement(action));
        }
    }
}
