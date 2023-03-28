using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class FastImmutableListTest : VerifyBase
    {
        public FastImmutableListTest() : base()
        {
        }

        [Fact]
        public void Concurrency()
        {
            var list = FastImmutableList<string>.Empty
                .Push("1")
                .Push("2");

            list
                .Push("3")
                .Should()
                .Equal("1", "2", "3");

            list
                .Push("4")
                .Should()
                .Equal("1", "2", "4");
        }
    }
}
