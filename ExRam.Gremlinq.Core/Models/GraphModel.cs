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
                        .Where(x => x.IsClass)
                        .Where(type => !type.IsAbstract)
                        .ToImmutableDictionary(
                            type => type,
                            type => new ElementMetadata(type.Name));
                }

                public IImmutableDictionary<Type, ElementMetadata> Metadata { get; }
            }

            public AssemblyGraphModel(Type vertexBaseType, Type edgeBaseType, IEnumerable<Assembly> assemblies, ILogger logger)
            {
                if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                    throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

                if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                    throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

                var assemblyArray = assemblies
                    .Append(vertexBaseType.Assembly)
                    .Append(edgeBaseType.Assembly)
                    .Distinct()
                    .ToArray();

                VerticesModel = new AssemblyGraphElementModel(vertexBaseType, assemblyArray, logger);
                EdgesModel = new AssemblyGraphElementModel(edgeBaseType, assemblyArray, logger);
                PropertiesModel = GraphElementPropertyModel.FromGraphElementModels(VerticesModel, EdgesModel);
            }

            public IGraphElementModel EdgesModel { get; }
            public IGraphElementModel VerticesModel { get; }
            public IGraphElementPropertyModel PropertiesModel { get; }
        }

        public static readonly IGraphModel Empty = new EmptyGraphModel();

        public static IGraphModel Dynamic(ILogger logger = null)
        {
            return FromBaseTypes<IVertex, IEdge>(logger, AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IGraphModel FromBaseTypes<TVertex, TEdge>(ILogger logger = null, params Assembly[] additionalAssemblies)
        {
            return FromBaseTypes(typeof(TVertex), typeof(TEdge), logger, additionalAssemblies);
        }

        public static IGraphModel FromBaseTypes(Type vertexBaseType, Type edgeBaseType, ILogger logger = null, params Assembly[] additionalAssemblies)
        {
            return new AssemblyGraphModel(vertexBaseType, edgeBaseType, additionalAssemblies, logger);
        }

        public static IGraphModel ConfigureElements(this IGraphModel model, Func<IGraphElementModel, IGraphElementModel> transformation)
        {
            return model
                .ConfigureVertices(transformation)
                .ConfigureEdges(transformation);
        }

        public static IGraphModel ConfigureVertices(this IGraphModel model, Func<IGraphElementModel, IGraphElementModel> transformation)
        {
            return new GraphModelImpl(
                transformation(model.VerticesModel),
                model.EdgesModel,
                model.PropertiesModel);
        }

        public static IGraphModel ConfigureEdges(this IGraphModel model, Func<IGraphElementModel, IGraphElementModel> transformation)
        {
            return new GraphModelImpl(
                model.VerticesModel,
                transformation(model.EdgesModel),
                model.PropertiesModel);
        }

        public static IGraphModel ConfigureProperties(this IGraphModel model, Func<IGraphElementPropertyModel, IGraphElementPropertyModel> transformation)
        {
            return new GraphModelImpl(
                model.VerticesModel,
                model.EdgesModel,
                transformation(model.PropertiesModel));
        }
    }
}
