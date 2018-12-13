using System.Reflection;
using ExRam.Gremlinq.GraphElements;
using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GraphModelTest
    {
        [Fact]
        public void FromAssembly_includes_abstract_types()
        {
            var model = GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge));

            model.TryGetLabel(typeof(Authority)).IsSome
                .Should()
                .BeTrue();
        }
    }
}
