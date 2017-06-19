using System.Reflection;
using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GraphModelTest
    {
        [Fact]
        public void FromAssembly_includes_abstract_types()
        {
            var model = GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple);

            model.VertexLabels.Keys
                .Should()
                .Contain(typeof(Authority));
        }
    }
}
