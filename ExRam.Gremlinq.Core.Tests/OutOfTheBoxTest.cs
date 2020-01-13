using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class OutOfTheBoxTest
    {
        private class SomeEntity
        {

        }

        [Fact]
        public async Task Execution()
        {
            (await g
                    .V()
                    .ToArrayAsync())
                .Should()
                .BeEmpty();
        }

        [Fact]
        public async Task V_SomeEntity()
        {
            g
                .ConfigureExecutionPipeline(x => x.UseSerializer(GremlinQuerySerializer.Groovy))
                .V<SomeEntity>()
                .Should()
                .SerializeToGroovy("g.V().hasLabel(_a)")
                .WithParameters("SomeEntity");
        }
    }
}
