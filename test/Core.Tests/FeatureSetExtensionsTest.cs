using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class FeatureSetExtensionsTest
    {
        [Fact]
        public void Supports_GraphFeatures_returns_true_when_present()
        {
            FeatureSet.Full
                .Supports(GraphFeatures.Transactions)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Supports_GraphFeatures_returns_false_when_absent()
        {
            FeatureSet.None
                .Supports(GraphFeatures.Transactions)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Supports_VariableFeatures_returns_true_when_present()
        {
            FeatureSet.Full
                .Supports(VariableFeatures.Variables)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Supports_VariableFeatures_returns_false_when_absent()
        {
            FeatureSet.None
                .Supports(VariableFeatures.Variables)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Supports_VertexFeatures_returns_true_when_present()
        {
            FeatureSet.Full
                .Supports(VertexFeatures.AddVertices)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Supports_VertexFeatures_returns_false_when_absent()
        {
            FeatureSet.None
                .Supports(VertexFeatures.AddVertices)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Supports_VertexPropertyFeatures_returns_true_when_present()
        {
            FeatureSet.Full
                .Supports(VertexPropertyFeatures.Properties)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Supports_VertexPropertyFeatures_returns_false_when_absent()
        {
            FeatureSet.None
                .Supports(VertexPropertyFeatures.Properties)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Supports_EdgeFeatures_returns_true_when_present()
        {
            FeatureSet.Full
                .Supports(EdgeFeatures.AddEdges)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Supports_EdgeFeatures_returns_false_when_absent()
        {
            FeatureSet.None
                .Supports(EdgeFeatures.AddEdges)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Supports_EdgePropertyFeatures_returns_true_when_present()
        {
            FeatureSet.Full
                .Supports(EdgePropertyFeatures.Properties)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Supports_EdgePropertyFeatures_returns_false_when_absent()
        {
            FeatureSet.None
                .Supports(EdgePropertyFeatures.Properties)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void ConfigureGraphFeatures()
        {
            var configured = FeatureSet.None
                .ConfigureGraphFeatures(static _ => GraphFeatures.Transactions);

            configured.GraphFeatures
                .Should()
                .Be(GraphFeatures.Transactions);
        }

        [Fact]
        public void ConfigureVariableFeatures()
        {
            var configured = FeatureSet.None
                .ConfigureVariableFeatures(static _ => VariableFeatures.Variables);

            configured.VariableFeatures
                .Should()
                .Be(VariableFeatures.Variables);
        }

        [Fact]
        public void ConfigureVertexFeatures()
        {
            var configured = FeatureSet.None
                .ConfigureVertexFeatures(static _ => VertexFeatures.AddVertices);

            configured.VertexFeatures
                .Should()
                .Be(VertexFeatures.AddVertices);
        }

        [Fact]
        public void ConfigureVertexPropertyFeatures()
        {
            var configured = FeatureSet.None
                .ConfigureVertexPropertyFeatures(static _ => VertexPropertyFeatures.Properties);

            configured.VertexPropertyFeatures
                .Should()
                .Be(VertexPropertyFeatures.Properties);
        }

        [Fact]
        public void ConfigureEdgeFeatures()
        {
            var configured = FeatureSet.None
                .ConfigureEdgeFeatures(static _ => EdgeFeatures.AddEdges);

            configured.EdgeFeatures
                .Should()
                .Be(EdgeFeatures.AddEdges);
        }

        [Fact]
        public void ConfigureEdgePropertyFeatures()
        {
            var configured = FeatureSet.None
                .ConfigureEdgePropertyFeatures(static _ => EdgePropertyFeatures.Properties);

            configured.EdgePropertyFeatures
                .Should()
                .Be(EdgePropertyFeatures.Properties);
        }
    }
}
