using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class GraphModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            public GraphModelImpl(IGraphElementModel verticesModel, IGraphElementModel edgesModel, IGraphElementPropertyModel propertiesModel)
            {
                VerticesModel = verticesModel;
                EdgesModel = edgesModel;
                PropertiesModel = propertiesModel;
            }

            public IGraphElementModel VerticesModel { get; }

            public IGraphElementModel EdgesModel { get; }

            public IGraphElementPropertyModel PropertiesModel { get; }
        }

        private sealed class EmptyGraphModel : IGraphModel
        {
            public IGraphElementModel VerticesModel { get => GraphElementModel.Empty; }
            public IGraphElementModel EdgesModel { get => GraphElementModel.Empty; }

            public IGraphElementPropertyModel PropertiesModel { get; } = GraphElementPropertyModel.Default;
        }

        private sealed class InvalidGraphModel : IGraphModel
        {
            public IGraphElementModel VerticesModel { get => GraphElementModel.Invalid; }
            public IGraphElementModel EdgesModel { get => GraphElementModel.Invalid; }
            public IGraphElementPropertyModel PropertiesModel { get => GraphElementPropertyModel.Invalid; }
        }

        private sealed class AssemblyGraphModel : IGraphModel
        {
            private sealed class AssemblyGraphElementModel : IGraphElementModel
            {
                public AssemblyGraphElementModel(Type baseType, IEnumerable<Assembly> assemblies, ILogger logger)
                {
                    Metadata = assemblies
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
                            type => ElementMetadata.Default);
                }

                public IImmutableDictionary<Type, ElementMetadata> Metadata { get; }
            }

            public AssemblyGraphModel(Type vertexBaseType, Type edgeBaseType, IEnumerable<Assembly> assemblies, ILogger logger)
            {
                if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                    throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

                if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                    throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

                var assemblyArray = assemblies.ToArray();

                VerticesModel = new AssemblyGraphElementModel(vertexBaseType, assemblyArray, logger);
                EdgesModel = new AssemblyGraphElementModel(edgeBaseType, assemblyArray, logger);
                PropertiesModel = GraphElementPropertyModel.FromGraphElementModels(VerticesModel, EdgesModel);
            }

            public IGraphElementModel EdgesModel { get; }
            public IGraphElementModel VerticesModel { get; }
            public IGraphElementPropertyModel PropertiesModel { get; }
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

        public static IGraphModel WithPropertiesModel(this IGraphModel model, Func<IGraphElementPropertyModel, IGraphElementPropertyModel> transformation)
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
