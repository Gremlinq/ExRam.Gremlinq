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

        public static IGraphModel FromBaseTypes<TVertexBaseType, TEdgeBaseType>(Func<IAssemblyLookupBuilder, IAssemblyLookupSet>? assemblyLookupTransformation = null)
        {
            return FromBaseTypesImpl<TVertexBaseType, TEdgeBaseType>(assemblyLookupTransformation ?? (static _ => _.IncludeAssembliesFromAppDomain()));
        }

        private static IGraphModel FromBaseTypesImpl<TVertexBaseType, TEdgeBaseType>(Func<IAssemblyLookupBuilder, IAssemblyLookupSet> assemblyLookupTransformation)
        {
            if (typeof(TVertexBaseType).IsAssignableFrom(typeof(TEdgeBaseType)))
                throw new ArgumentException($"{typeof(TVertexBaseType)} may not be in the inheritance hierarchy of {typeof(TEdgeBaseType)}.");

            if (typeof(TEdgeBaseType).IsAssignableFrom(typeof(TVertexBaseType)))
                throw new ArgumentException($"{typeof(TEdgeBaseType)} may not be in the inheritance hierarchy of {typeof(TVertexBaseType)}.");

            var assemblies = assemblyLookupTransformation(new AssemblyLookupSet(new[] { typeof(TVertexBaseType), typeof(TEdgeBaseType) }, ImmutableHashSet<Assembly>.Empty))
                .Assemblies;

            var verticesModel = GraphElementModel.FromBaseType<TVertexBaseType>(assemblies);
            var edgesModel = GraphElementModel.FromBaseType<TEdgeBaseType>(assemblies);

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
