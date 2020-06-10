namespace ExRam.Gremlinq.Core
{
    public static class FeatureSetExtensions
    {
        public static bool Supports(this IFeatureSet featureSet, GraphFeatures graphFeatures) => (featureSet.GraphFeatures & graphFeatures) == graphFeatures;

        public static bool Supports(this IFeatureSet featureSet, VariableFeatures variableFeatures) => (featureSet.VariableFeatures & variableFeatures) == variableFeatures;

        public static bool Supports(this IFeatureSet featureSet, VertexFeatures vertexFeatures) => (featureSet.VertexFeatures & vertexFeatures) == vertexFeatures;

        public static bool Supports(this IFeatureSet featureSet, VertexPropertyFeatures vertexPropertyFeatures) => (featureSet.VertexPropertyFeatures & vertexPropertyFeatures) == vertexPropertyFeatures;

        public static bool Supports(this IFeatureSet featureSet, EdgeFeatures edgeFeatures) => (featureSet.EdgeFeatures & edgeFeatures) == edgeFeatures;

        public static bool Supports(this IFeatureSet featureSet, EdgePropertyFeatures edgePropertyFeatures) => (featureSet.EdgePropertyFeatures & edgePropertyFeatures) == edgePropertyFeatures;
    }
}
