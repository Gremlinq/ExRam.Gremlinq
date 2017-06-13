using System.Reflection;
using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GraphSchemaTest
    {
        [Fact]
        public void FromAssembly_ToGraphSchema_does_not_include_abstract_types()
        {
            var model = GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple);

            model.ToGraphSchema().VertexSchemaInfos
                .Should()
                .NotContain(x => x.Label == "Authority");
        }
    }
}