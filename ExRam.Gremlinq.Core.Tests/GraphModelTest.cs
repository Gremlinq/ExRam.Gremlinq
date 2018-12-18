using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GraphModelTest
    {
        [Fact]
        public void FromAssembly_includes_abstract_types()
        {
            var model = GraphModel.Dynamic();

            model.TryGetLabel(typeof(Authority)).IsSome
                .Should()
                .BeTrue();
        }
    }
}
