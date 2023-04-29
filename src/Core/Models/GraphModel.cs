using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
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

            public IGraphModel ConfigureVertices(Func<IGraphElementModel, IGraphElementModel> transformation)
            {
                return new GraphModelImpl(
                    transformation(VerticesModel),
                    EdgesModel,
                    PropertiesModel);
            }

            public IGraphModel ConfigureEdges(Func<IGraphElementModel, IGraphElementModel> transformation)
            {
                return new GraphModelImpl(
                    VerticesModel,
                    transformation(EdgesModel),
                    PropertiesModel);
            }

            public IGraphModel ConfigureProperties(Func<IGraphElementPropertyModel, IGraphElementPropertyModel> transformation)
            {
                return new GraphModelImpl(
                    VerticesModel,
                    EdgesModel,
                    transformation(PropertiesModel));
            }

            public IGraphElementModel VerticesModel { get; }
            public IGraphElementModel EdgesModel { get; }
            public IGraphElementPropertyModel PropertiesModel { get; }
        }

        public static readonly IGraphModel Empty = new GraphModelImpl(
            GraphElementModel.Empty,
            GraphElementModel.Empty,
            GraphElementPropertyModel.Empty);

        public static readonly IGraphModel Invalid = new GraphModelImpl(
            GraphElementModel.Invalid,
            GraphElementModel.Invalid,
            GraphElementPropertyModel.Invalid);

        public static IGraphModel FromBaseTypes<TVertex, TEdge>(Func<IAssemblyLookupBuilder, IAssemblyLookupSet>? assemblyLookupTransformation = null)
        {
            return FromBaseTypes(typeof(TVertex), typeof(TEdge), assemblyLookupTransformation ?? (static _ => _.IncludeAssembliesFromAppDomain()));
        }

        public static IGraphModel FromBaseTypes(Type vertexBaseType, Type edgeBaseType, Func<IAssemblyLookupBuilder, IAssemblyLookupSet> assemblyLookupTransformation)
        {
            if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

            if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

            var assemblies = assemblyLookupTransformation(new AssemblyLookupSet(new[] { vertexBaseType, edgeBaseType }, ImmutableHashSet<Assembly>.Empty))
                .Assemblies;

            var verticesModel = GraphElementModel.FromBaseType(vertexBaseType, assemblies);
            var edgesModel = GraphElementModel.FromBaseType(edgeBaseType, assemblies);

            return new GraphModelImpl(
                verticesModel,
                edgesModel,
                GraphElementPropertyModel.FromGraphElementModels(verticesModel, edgesModel));
        }

        public static IGraphModel FromTypes(Type[] vertexTypes, Type[] edgeTypes)
        {
            var verticesModel = GraphElementModel.FromTypes(vertexTypes);
            var edgesModel = GraphElementModel.FromTypes(edgeTypes);

            return new GraphModelImpl(
                verticesModel,
                edgesModel,
                GraphElementPropertyModel.FromGraphElementModels(verticesModel, edgesModel));
        }

        public static IGraphModel ConfigureElements(this IGraphModel model, Func<IGraphElementModel, IGraphElementModel> transformation)
        {
            return model
                .ConfigureVertices(transformation)
                .ConfigureEdges(transformation);
        }
    }
}
