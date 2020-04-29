namespace ExRam.Gremlinq.Core
{
    public static class FeatureSetExtensions
    {
        public static bool Supports(this FeatureSet featureSet, GraphFeatures graphFeatures) => (featureSet.GraphFeatures & graphFeatures) == graphFeatures;

        public static bool Supports(this FeatureSet featureSet, VariableFeatures variableFeatures) => (featureSet.VariableFeatures & variableFeatures) == variableFeatures;

        public static bool Supports(this FeatureSet featureSet, VertexFeatures vertexFeatures) => (featureSet.VertexFeatures & vertexFeatures) == vertexFeatures;

        public static bool Supports(this FeatureSet featureSet, VertexPropertyFeatures vertexPropertyFeatures) => (featureSet.VertexPropertyFeatures & vertexPropertyFeatures) == vertexPropertyFeatures;

        public static bool Supports(this FeatureSet featureSet, EdgeFeatures edgeFeatures) => (featureSet.EdgeFeatures & edgeFeatures) == edgeFeatures;

        public static bool Supports(this FeatureSet featureSet, EdgePropertyFeatures edgePropertyFeatures) => (featureSet.EdgePropertyFeatures & edgePropertyFeatures) == edgePropertyFeatures;
    }
}
