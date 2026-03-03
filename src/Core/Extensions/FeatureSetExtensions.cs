namespace ExRam.Gremlinq.Core
{
    public static class FeatureSetExtensions
    {
        /// <summary>
        /// Determines whether the feature set supports the specified graph features.
        /// </summary>
        /// <param name="featureSet">The feature set to query.</param>
        /// <param name="graphFeatures">The graph features to check for.</param>
        public static bool Supports(this IFeatureSet featureSet, GraphFeatures graphFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.GraphFeatures & graphFeatures) == graphFeatures;
        }

        /// <summary>
        /// Determines whether the feature set supports the specified variable features.
        /// </summary>
        /// <param name="featureSet">The feature set to query.</param>
        /// <param name="variableFeatures">The variable features to check for.</param>
        public static bool Supports(this IFeatureSet featureSet, VariableFeatures variableFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.VariableFeatures & variableFeatures) == variableFeatures;
        }

        /// <summary>
        /// Determines whether the feature set supports the specified vertex features.
        /// </summary>
        /// <param name="featureSet">The feature set to query.</param>
        /// <param name="vertexFeatures">The vertex features to check for.</param>
        public static bool Supports(this IFeatureSet featureSet, VertexFeatures vertexFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.VertexFeatures & vertexFeatures) == vertexFeatures;
        }

        /// <summary>
        /// Determines whether the feature set supports the specified vertex property features.
        /// </summary>
        /// <param name="featureSet">The feature set to query.</param>
        /// <param name="vertexPropertyFeatures">The vertex property features to check for.</param>
        public static bool Supports(this IFeatureSet featureSet, VertexPropertyFeatures vertexPropertyFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.VertexPropertyFeatures & vertexPropertyFeatures) == vertexPropertyFeatures;
        }

        /// <summary>
        /// Determines whether the feature set supports the specified edge features.
        /// </summary>
        /// <param name="featureSet">The feature set to query.</param>
        /// <param name="edgeFeatures">The edge features to check for.</param>
        public static bool Supports(this IFeatureSet featureSet, EdgeFeatures edgeFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.EdgeFeatures & edgeFeatures) == edgeFeatures;
        }

        /// <summary>
        /// Determines whether the feature set supports the specified edge property features.
        /// </summary>
        /// <param name="featureSet">The feature set to query.</param>
        /// <param name="edgePropertyFeatures">The edge property features to check for.</param>
        public static bool Supports(this IFeatureSet featureSet, EdgePropertyFeatures edgePropertyFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.EdgePropertyFeatures & edgePropertyFeatures) == edgePropertyFeatures;
        }
    }
}
