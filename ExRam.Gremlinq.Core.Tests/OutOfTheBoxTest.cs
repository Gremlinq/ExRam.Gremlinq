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
                .ConfigureEnvironment(e => e
                    .ConfigureExecutionPipeline(p => p.ConfigureSerializer(s => s.ToGroovy())))
                .V<SomeEntity>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("SomeEntity");
        }
    }
}
