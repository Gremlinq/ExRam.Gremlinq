using System.Linq;
using System.Threading.Tasks;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class ExecutionPipelinesTest
    {
        [Fact]
        public async Task Echo()
        {
            var query = await g
                .UseModel(GraphModel
                    .FromBaseTypes<Vertex, Edge>())
                .UseExecutionPipeline(_ => _
                    .EchoGremlinQueryAsString())
                .V<Person>()
                .Where(x => x.Age == 36)
                .Cast<string>()
                .FirstAsync();

            query
                .Should()
                .Be("g.V().hasLabel(_a).has(_b, _c)");
        }
    }
}
