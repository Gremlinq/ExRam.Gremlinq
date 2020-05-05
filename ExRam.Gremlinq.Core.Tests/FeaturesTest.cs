using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public class FeaturesTest
    {
        [Fact]
        public void EdgeFeatures_all()
        {
            Enum.GetValues(typeof(EdgeFeatures)).Cast<int>().Sum()
                .Should()
                .Be(2 * (int)EdgeFeatures.All);
        }

        [Fact]
        public void EdgePropertyFeatures_all()
        {
            Enum.GetValues(typeof(EdgePropertyFeatures)).Cast<int>().Sum()
                .Should()
                .Be(2 * (int)EdgePropertyFeatures.All);
        }

        [Fact]
        public void GraphFeatures_all()
        {
            Enum.GetValues(typeof(GraphFeatures)).Cast<int>().Sum()
                .Should()
                .Be(2 * (int)GraphFeatures.All);
        }

        [Fact]
        public void VariableFeatures_all()
        {
            Enum.GetValues(typeof(VariableFeatures)).Cast<int>().Sum()
                .Should()
                .Be(2 * (int)VariableFeatures.All);
        }

        [Fact]
        public void VertexFeatures_all()
        {
            Enum.GetValues(typeof(VertexFeatures)).Cast<int>().Sum()
                .Should()
                .Be(2 * (int)VertexFeatures.All);
        }

        [Fact]
        public void VertexPropertyFeatures_all()
        {
            Enum.GetValues(typeof(VertexPropertyFeatures)).Cast<int>().Sum()
                .Should()
                .Be(2 * (int)VertexPropertyFeatures.All);
        }
    }
}