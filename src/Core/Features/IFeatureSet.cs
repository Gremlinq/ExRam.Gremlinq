namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Describes the capabilities of a graph database as a set of feature flags.
    /// Corresponds to the TinkerPop Graph.Features.
    /// </summary>
    public interface IFeatureSet
    {
        /// <summary>
        /// Configures graph features by applying a transformation.
        /// </summary>
        IFeatureSet ConfigureGraphFeatures(Func<GraphFeatures, GraphFeatures> config);

        /// <summary>
        /// Configures variable features by applying a transformation.
        /// </summary>
        IFeatureSet ConfigureVariableFeatures(Func<VariableFeatures, VariableFeatures> config);

        /// <summary>
        /// Configures vertex features by applying a transformation.
        /// </summary>
        IFeatureSet ConfigureVertexFeatures(Func<VertexFeatures, VertexFeatures> config);

        /// <summary>
        /// Configures vertex property features by applying a transformation.
        /// </summary>
        IFeatureSet ConfigureVertexPropertyFeatures(Func<VertexPropertyFeatures, VertexPropertyFeatures> config);

        /// <summary>
        /// Configures edge features by applying a transformation.
        /// </summary>
        IFeatureSet ConfigureEdgeFeatures(Func<EdgeFeatures, EdgeFeatures> config);

        /// <summary>
        /// Configures edge property features by applying a transformation.
        /// </summary>
        IFeatureSet ConfigureEdgePropertyFeatures(Func<EdgePropertyFeatures, EdgePropertyFeatures> config);

        /// <summary>
        /// Gets the graph-level features.
        /// </summary>
        GraphFeatures GraphFeatures { get; }

        /// <summary>
        /// Gets the edge property features.
        /// </summary>
        EdgePropertyFeatures EdgePropertyFeatures { get; }

        /// <summary>
        /// Gets the edge features.
        /// </summary>
        EdgeFeatures EdgeFeatures { get; }

        /// <summary>
        /// Gets the variable features.
        /// </summary>
        VariableFeatures VariableFeatures { get; }

        /// <summary>
        /// Gets the vertex features.
        /// </summary>
        VertexFeatures VertexFeatures { get; }

        /// <summary>
        /// Gets the vertex property features.
        /// </summary>
        VertexPropertyFeatures VertexPropertyFeatures { get; }
    }
}
