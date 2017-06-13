using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

            model.VertexTypes.Keys
                .Should()
                .Contain(typeof(Authority));
        }
    }
}
