using System;

namespace ExRam.Gremlinq.Core
{
    public static class FeatureSet
    {
        private sealed class FeatureSetImpl : IFeatureSet
        {
            public FeatureSetImpl(
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

            public IFeatureSet ConfigureGraphFeatures(Func<GraphFeatures, GraphFeatures> config)
            {
                return new FeatureSetImpl(config(GraphFeatures), VariableFeatures, VertexFeatures, VertexPropertyFeatures, EdgeFeatures, EdgePropertyFeatures);
            }

            public IFeatureSet ConfigureVariableFeatures(Func<VariableFeatures, VariableFeatures> config)
            {
                return new FeatureSetImpl(GraphFeatures, config(VariableFeatures), VertexFeatures, VertexPropertyFeatures, EdgeFeatures, EdgePropertyFeatures);
            }

            public IFeatureSet ConfigureVertexFeatures(Func<VertexFeatures, VertexFeatures> config)
            {
                return new FeatureSetImpl(GraphFeatures, VariableFeatures, config(VertexFeatures), VertexPropertyFeatures, EdgeFeatures, EdgePropertyFeatures);
            }

            public IFeatureSet ConfigureVertexPropertyFeatures(Func<VertexPropertyFeatures, VertexPropertyFeatures> config)
            {
                return new FeatureSetImpl(GraphFeatures, VariableFeatures, VertexFeatures, config(VertexPropertyFeatures), EdgeFeatures, EdgePropertyFeatures);
            }

            public IFeatureSet ConfigureEdgeFeatures(Func<EdgeFeatures, EdgeFeatures> config)
            {
                return new FeatureSetImpl(GraphFeatures, VariableFeatures, VertexFeatures, VertexPropertyFeatures, config(EdgeFeatures), EdgePropertyFeatures);
            }

            public IFeatureSet ConfigureEdgePropertyFeatures(Func<EdgePropertyFeatures, EdgePropertyFeatures> config)
            {
                return new FeatureSetImpl(GraphFeatures, VariableFeatures, VertexFeatures, VertexPropertyFeatures, EdgeFeatures, config(EdgePropertyFeatures));
            }

            public GraphFeatures GraphFeatures { get; }
            public EdgePropertyFeatures EdgePropertyFeatures { get; }
            public EdgeFeatures EdgeFeatures { get; }
            public VariableFeatures VariableFeatures { get; }
            public VertexFeatures VertexFeatures { get; }
            public VertexPropertyFeatures VertexPropertyFeatures { get; }
        }

        public static IFeatureSet Full = new FeatureSetImpl(
            GraphFeatures.All,
            VariableFeatures.All,
            VertexFeatures.All,
            VertexPropertyFeatures.All,
            EdgeFeatures.All,
            EdgePropertyFeatures.All);

        public static IFeatureSet None = new FeatureSetImpl(
            GraphFeatures.None,
            VariableFeatures.None,
            VertexFeatures.None,
            VertexPropertyFeatures.None,
            EdgeFeatures.None,
            EdgePropertyFeatures.None);
    }
}
