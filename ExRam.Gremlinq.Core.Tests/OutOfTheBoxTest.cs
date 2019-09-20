using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class OutOfTheBoxTest
    {
        [Fact]
        public async Task Execution()
        {
            (await g
                    .V()
                    .ToArrayAsync())
                .Should()
                .BeEmpty();
        }
    }
}
