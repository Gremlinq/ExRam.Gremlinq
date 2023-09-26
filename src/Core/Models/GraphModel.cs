using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public static class GraphModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            public GraphModelImpl(IGraphElementModel verticesModel, IGraphElementModel edgesModel)
            {
                VerticesModel = verticesModel;
                EdgesModel = edgesModel;
            }

            public IGraphModel ConfigureVertices(Func<IGraphElementModel, IGraphElementModel> transformation) => new GraphModelImpl(
                transformation(VerticesModel),
                EdgesModel);

            public IGraphModel ConfigureEdges(Func<IGraphElementModel, IGraphElementModel> transformation) => new GraphModelImpl(
                VerticesModel,
                transformation(EdgesModel));

            public IGraphModel AddAssemblies(params Assembly[] assemblies) => this
                .ConfigureElements(__ => __.AddAssemblies(assemblies));

            public IGraphElementModel VerticesModel { get; }
            public IGraphElementModel EdgesModel { get; }
        }

        public static readonly IGraphModel Invalid = new GraphModelImpl(
            GraphElementModel.Invalid,
            GraphElementModel.Invalid);

        public static IGraphModel FromBaseTypes<TVertexBaseType, TEdgeBaseType>()
        {
            if (typeof(TVertexBaseType).IsAssignableFrom(typeof(TEdgeBaseType)))
                throw new ArgumentException($"{typeof(TVertexBaseType)} may not be in the inheritance hierarchy of {typeof(TEdgeBaseType)}.");

            if (typeof(TEdgeBaseType).IsAssignableFrom(typeof(TVertexBaseType)))
                throw new ArgumentException($"{typeof(TEdgeBaseType)} may not be in the inheritance hierarchy of {typeof(TVertexBaseType)}.");

            var verticesModel = GraphElementModel.FromBaseType<TVertexBaseType>();
            var edgesModel = GraphElementModel.FromBaseType<TEdgeBaseType>();

            return new GraphModelImpl(
                verticesModel,
                edgesModel);
        }

        public static IGraphModel ConfigureElements(this IGraphModel model, Func<IGraphElementModel, IGraphElementModel> transformation) => model
            .ConfigureVertices(transformation)
            .ConfigureEdges(transformation);
    }
}
