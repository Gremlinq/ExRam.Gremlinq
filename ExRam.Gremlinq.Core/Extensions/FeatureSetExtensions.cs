namespace ExRam.Gremlinq.Core
{
    public static class FeatureSetExtensions
    {
        public static bool Supports(this FeatureSet featureSet, GraphFeatures graphFeatures) => featureSet.GraphFeatures.HasFlag(graphFeatures);

        public static bool Supports(this FeatureSet featureSet, VariableFeatures variableFeatures) => featureSet.VariableFeatures.HasFlag(variableFeatures);

        public static bool Supports(this FeatureSet featureSet, VertexFeatures vertexFeatures) => featureSet.VertexFeatures.HasFlag(vertexFeatures);

        public static bool Supports(this FeatureSet featureSet, VertexPropertyFeatures vertexPropertyFeatures) => featureSet.VertexPropertyFeatures.HasFlag(vertexPropertyFeatures);

        public static bool Supports(this FeatureSet featureSet, EdgeFeatures edgeFeatures) => featureSet.EdgeFeatures.HasFlag(edgeFeatures);

        public static bool Supports(this FeatureSet featureSet, EdgePropertyFeatures edgePropertyFeatures) => featureSet.EdgePropertyFeatures.HasFlag(edgePropertyFeatures);
    }
}
