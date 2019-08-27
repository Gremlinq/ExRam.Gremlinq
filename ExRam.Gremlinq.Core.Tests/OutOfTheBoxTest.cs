using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class OutOfTheBoxTest
    {
        [Fact]
        public void Serialization()
        {
            g
                .V()
                .Should()
                .SerializeToGroovy("g.V()");
        }

        [Fact]
        public void Execution()
        {
            g
                .V()
                .Awaiting(async _ => await _
                    .ToArrayAsync())
                .Should()
                .Throw<InvalidOperationException>();
        }
    }
}
