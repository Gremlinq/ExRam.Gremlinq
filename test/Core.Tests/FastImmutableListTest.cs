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
        public void Slice()
        {
            var list = FastImmutableList<string>.Empty
                .Push("1", "2", "3", "4");

            list
                .Slice(1, 2)
                .AsSpan()
                .ToArray()
                .Should()
                .Equal("2", "3");
        }

        [Fact]
        public void Slice_all()
        {
            var list = FastImmutableList<string>.Empty
                .Push("1", "2", "3", "4");

            list
                .Slice(0, 4)
                .AsSpan()
                .ToArray()
                .Should()
                .Equal("1", "2", "3", "4");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(4)]
        public void Slice_of_length_zero(int start)
        {
            var list = FastImmutableList<string>.Empty
                .Push("1", "2", "3", "4");

            list
                .Slice(start, 0)
                .Count
                .Should()
                .Be(0);
        }

        [Fact]
        public void Slice_of_empty()
        {
            FastImmutableList<string>.Empty
                .Slice(0, 0)
                .Count
                .Should()
                .Be(0);

            default(FastImmutableList<string>)
                .Slice(0, 0)
                .Count
                .Should()
                .Be(0);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(-1, 1)]
        [InlineData(-1, 5)]
        [InlineData(int.MinValue, 0)]
        //Beyond the items of the list, but within the capacity of the memory underneath it.
        [InlineData(5, 0)]
        [InlineData(5, -1)]
        [InlineData(16, 0)]
        //Beyond the capacity of the memory underneath the list.
        [InlineData(17, 0)]
        [InlineData(int.MaxValue, 0)]
        public void Slice_start_out_of_range(int start, int length)
        {
            var list = FastImmutableList<string>.Empty
                .Push("1", "2", "3", "4");

            FluentActions
                .Invoking(() => list.Slice(start, length))
                .Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithParameterName("start");
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(1, -1)]
        [InlineData(4, -1)]
        [InlineData(0, int.MinValue)]
        [InlineData(0, 5)]
        [InlineData(1, 4)]
        [InlineData(3, 2)]
        [InlineData(4, 1)]
        [InlineData(1, int.MaxValue)]
        public void Slice_length_out_of_range(int start, int length)
        {
            var list = FastImmutableList<string>.Empty
                .Push("1", "2", "3", "4");

            FluentActions
                .Invoking(() => list.Slice(start, length))
                .Should()
                .Throw<ArgumentOutOfRangeException>()
                .WithParameterName("length");
        }

        [Fact]
        public void Slice_of_empty_out_of_range()
        {
            FluentActions
                .Invoking(() => FastImmutableList<string>.Empty.Slice(0, 1))
                .Should()
                .Throw<ArgumentOutOfRangeException>();

            FluentActions
                .Invoking(() => FastImmutableList<string>.Empty.Slice(0, -1))
                .Should()
                .Throw<ArgumentOutOfRangeException>();

            FluentActions
                .Invoking(() => FastImmutableList<string>.Empty.Slice(1, 0))
                .Should()
                .Throw<ArgumentOutOfRangeException>();
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
