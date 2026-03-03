using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    /// <summary>
    /// Describes the graph schema by providing separate models for vertices and edges.
    /// </summary>
    public interface IGraphModel
    {
        /// <summary>
        /// Scans the specified assemblies for vertex and edge types and adds them to the model.
        /// </summary>
        /// <param name="assemblies">The assemblies to scan.</param>
        IGraphModel AddAssemblies(params Assembly[] assemblies);

        /// <summary>
        /// Configures the edge element model by applying the specified transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the edges model.</param>
        IGraphModel ConfigureEdges(Func<IGraphElementModel, IGraphElementModel> transformation);

        /// <summary>
        /// Configures the vertex element model by applying the specified transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the vertices model.</param>
        IGraphModel ConfigureVertices(Func<IGraphElementModel, IGraphElementModel> transformation);

        /// <summary>
        /// Gets the model describing edge types.
        /// </summary>
        IGraphElementModel EdgesModel { get; }

        /// <summary>
        /// Gets the model describing vertex types.
        /// </summary>
        IGraphElementModel VerticesModel { get; }
    }
}
