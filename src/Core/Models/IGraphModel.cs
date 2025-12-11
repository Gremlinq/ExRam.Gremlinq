using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    /// <summary>
    /// Represents a graph model that defines the structure of vertices and edges in a graph database.
    /// </summary>
    public interface IGraphModel
    {
        /// <summary>
        /// Adds types from the specified assemblies to the graph model.
        /// </summary>
        /// <param name="assemblies">The assemblies containing types to add to the model.</param>
        /// <returns>A new graph model with the additional types.</returns>
        IGraphModel AddAssemblies(params Assembly[] assemblies);

        /// <summary>
        /// Configures the edge model by applying a transformation function.
        /// </summary>
        /// <param name="transformation">A function that transforms the edge model.</param>
        /// <returns>A new graph model with the transformed edge model.</returns>
        IGraphModel ConfigureEdges(Func<IGraphElementModel, IGraphElementModel> transformation);
        
        /// <summary>
        /// Configures the vertex model by applying a transformation function.
        /// </summary>
        /// <param name="transformation">A function that transforms the vertex model.</param>
        /// <returns>A new graph model with the transformed vertex model.</returns>
        IGraphModel ConfigureVertices(Func<IGraphElementModel, IGraphElementModel> transformation);

        /// <summary>
        /// Gets the model for edge elements in the graph.
        /// </summary>
        IGraphElementModel EdgesModel { get; }
        
        /// <summary>
        /// Gets the model for vertex elements in the graph.
        /// </summary>
        IGraphElementModel VerticesModel { get; }
    }
}
