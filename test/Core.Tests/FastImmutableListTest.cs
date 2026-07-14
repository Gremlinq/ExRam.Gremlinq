using ExRam.Gremlinq.Core.Steps;
using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class FastImmutableListTest
    {
        [Fact]
        public void Concurrency()
        {
            var list = FastImmutableList<string>.Empty
                .Push("1")
                .Push("2");

            list
                .Push("3")
                .AsSpan()
                .ToArray()
                .Should()
                .Equal("1", "2", "3");

            list
                .Push("4")
                .AsSpan()
                .ToArray()
                .Should()
                .Equal("1", "2", "4");
        }

        [Fact]
        public void Push_params()
        {
            var list = FastImmutableList<string>.Empty
                .Push("1");

            list = list
                .Push("2", "3", "4");

            list
                .AsSpan()
                .ToArray()
                .Should()
                .Equal("1", "2", "3", "4");
        }

        [Fact]
        public void Push_empty()
        {
            var list = FastImmutableList<string>.Empty
                .Push("1");

            list = list
                .Push([]);

            list
                .AsSpan()
                .ToArray()
                .Should()
                .Equal("1");
        }

        [Fact]
        public void Push_multiple() => _ = FastImmutableList<Step>.Empty
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local)
            .Push(CountStep.Local, CountStep.Local, CountStep.Local, CountStep.Local);
    }
}
