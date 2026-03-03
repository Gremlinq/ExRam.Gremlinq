using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    /// <summary>
    /// Provides factory methods and extension methods for <see cref="IGraphModel"/>.
    /// </summary>
    public static class GraphModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            public GraphModelImpl(IGraphElementModel verticesModel, IGraphElementModel edgesModel)
            {
                EdgesModel = edgesModel;
                VerticesModel = verticesModel;
            }

            public IGraphModel ConfigureVertices(Func<IGraphElementModel, IGraphElementModel> transformation) => new GraphModelImpl(
                transformation(VerticesModel),
                EdgesModel);

            public IGraphModel ConfigureEdges(Func<IGraphElementModel, IGraphElementModel> transformation) => new GraphModelImpl(
                VerticesModel,
                transformation(EdgesModel));

            public IGraphModel AddAssemblies(params Assembly[] assemblies) => this
                .ConfigureElements(_ => _.AddAssemblies(assemblies));

            public IGraphElementModel EdgesModel { get; }
            public IGraphElementModel VerticesModel { get; }
        }

        /// <summary>
        /// A graph model that has no types configured. Used as the default when no model has been set.
        /// </summary>
        public static readonly IGraphModel Invalid = new GraphModelImpl(GraphElementModel.Invalid, GraphElementModel.Invalid);

        /// <summary>
        /// Creates a graph model from the specified base types for vertices and edges.
        /// The base types must not be in each other's inheritance hierarchy.
        /// </summary>
        /// <typeparam name="TVertexBaseType">The base type for all vertex types.</typeparam>
        /// <typeparam name="TEdgeBaseType">The base type for all edge types.</typeparam>
        public static IGraphModel FromBaseTypes<TVertexBaseType, TEdgeBaseType>()
        {
            if (typeof(TVertexBaseType).IsAssignableFrom(typeof(TEdgeBaseType)))
                throw new ArgumentException($"{typeof(TVertexBaseType)} may not be in the inheritance hierarchy of {typeof(TEdgeBaseType)}.");

            if (typeof(TEdgeBaseType).IsAssignableFrom(typeof(TVertexBaseType)))
                throw new ArgumentException($"{typeof(TEdgeBaseType)} may not be in the inheritance hierarchy of {typeof(TVertexBaseType)}.");

            return new GraphModelImpl(
                GraphElementModel.FromBaseType<TVertexBaseType>(),
                GraphElementModel.FromBaseType<TEdgeBaseType>());
        }

        /// <summary>
        /// Applies a transformation to both the vertex and edge element models.
        /// </summary>
        /// <param name="model">The graph model to configure.</param>
        /// <param name="transformation">A function that transforms each element model.</param>
        public static IGraphModel ConfigureElements(this IGraphModel model, Func<IGraphElementModel, IGraphElementModel> transformation)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(transformation);

            return model
                .ConfigureVertices(transformation)
                .ConfigureEdges(transformation);
        }
    }
}
