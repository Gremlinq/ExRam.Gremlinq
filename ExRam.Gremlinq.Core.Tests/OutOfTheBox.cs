using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public class OutOfTheBox
    {
        [Fact]
        public void Serialization()
        {
            GremlinQuerySource.g
                .V()
                .Should()
                .SerializeToGroovy("g.V()");
        }

        [Fact]
        public void Execution()
        {
            GremlinQuerySource.g
                .V()
                .Awaiting(async _ => await _
                    .ToArrayAsync())
                .Should()
                .Throw<InvalidOperationException>();
        }
    }
}
