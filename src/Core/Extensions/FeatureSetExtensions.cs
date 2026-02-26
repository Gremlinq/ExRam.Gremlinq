namespace ExRam.Gremlinq.Core
{
    public static class FeatureSetExtensions
    {
        public static bool Supports(this IFeatureSet featureSet, GraphFeatures graphFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.GraphFeatures & graphFeatures) == graphFeatures;
        }

        public static bool Supports(this IFeatureSet featureSet, VariableFeatures variableFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.VariableFeatures & variableFeatures) == variableFeatures;
        }

        public static bool Supports(this IFeatureSet featureSet, VertexFeatures vertexFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.VertexFeatures & vertexFeatures) == vertexFeatures;
        }

        public static bool Supports(this IFeatureSet featureSet, VertexPropertyFeatures vertexPropertyFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.VertexPropertyFeatures & vertexPropertyFeatures) == vertexPropertyFeatures;
        }

        public static bool Supports(this IFeatureSet featureSet, EdgeFeatures edgeFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.EdgeFeatures & edgeFeatures) == edgeFeatures;
        }

        public static bool Supports(this IFeatureSet featureSet, EdgePropertyFeatures edgePropertyFeatures)
        {
            ArgumentNullException.ThrowIfNull(featureSet);

            return (featureSet.EdgePropertyFeatures & edgePropertyFeatures) == edgePropertyFeatures;
        }
    }
}
