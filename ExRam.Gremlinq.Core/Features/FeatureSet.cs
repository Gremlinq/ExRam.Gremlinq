using System;

namespace ExRam.Gremlinq.Core
{
    public readonly struct FeatureSet
    {
        public static FeatureSet Full = new FeatureSet(
            GraphFeatures.All,
            VariableFeatures.All,
            VertexFeatures.All,
            VertexPropertyFeatures.All,
            EdgeFeatures.All,
            EdgePropertyFeatures.All);

        public FeatureSet(
            GraphFeatures graphFeatures,
            VariableFeatures variableFeatures,
            VertexFeatures vertexFeatures,
            VertexPropertyFeatures vertexPropertyFeatures,
            EdgeFeatures edgeFeatures,
            EdgePropertyFeatures edgePropertyFeatures)
        {
            GraphFeatures = graphFeatures;
            VariableFeatures = variableFeatures;
            VertexFeatures = vertexFeatures;
            VertexPropertyFeatures = vertexPropertyFeatures;
            EdgeFeatures = edgeFeatures;
            EdgePropertyFeatures = edgePropertyFeatures;
        }

        public FeatureSet ConfigureGraphFeatures(Func<GraphFeatures, GraphFeatures> config)
        {
            return new FeatureSet(config(GraphFeatures), VariableFeatures, VertexFeatures, VertexPropertyFeatures, EdgeFeatures, EdgePropertyFeatures);
        }

        public FeatureSet ConfigureVariableFeatures(Func<VariableFeatures, VariableFeatures> config)
        {
            return new FeatureSet(GraphFeatures, config(VariableFeatures), VertexFeatures, VertexPropertyFeatures, EdgeFeatures, EdgePropertyFeatures);
        }

        public FeatureSet ConfigureVertexFeatures(Func<VertexFeatures, VertexFeatures> config)
        {
            return new FeatureSet(GraphFeatures, VariableFeatures, config(VertexFeatures), VertexPropertyFeatures, EdgeFeatures, EdgePropertyFeatures);
        }

        public FeatureSet ConfigureVertexPropertyFeatures(Func<VertexPropertyFeatures, VertexPropertyFeatures> config)
        {
            return new FeatureSet(GraphFeatures, VariableFeatures, VertexFeatures, config(VertexPropertyFeatures), EdgeFeatures, EdgePropertyFeatures);
        }

        public FeatureSet ConfigureEdgeFeatures(Func<EdgeFeatures, EdgeFeatures> config)
        {
            return new FeatureSet(GraphFeatures, VariableFeatures, VertexFeatures, VertexPropertyFeatures, config(EdgeFeatures), EdgePropertyFeatures);
        }

        public FeatureSet ConfigureEdgePropertyFeatures(Func<EdgePropertyFeatures, EdgePropertyFeatures> config)
        {
            return new FeatureSet(GraphFeatures, VariableFeatures, VertexFeatures, VertexPropertyFeatures, EdgeFeatures, config(EdgePropertyFeatures));
        }

        public GraphFeatures GraphFeatures { get; }
        public EdgePropertyFeatures EdgePropertyFeatures { get; }
        public EdgeFeatures EdgeFeatures { get; }
        public VariableFeatures VariableFeatures { get; }
        public VertexFeatures VertexFeatures { get; }
        public VertexPropertyFeatures VertexPropertyFeatures { get; }
    }
}
