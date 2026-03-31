using FluentAssertions;

using NSubstitute;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class PoolConfigurationTests
    {
        [Fact]
        public void WithPoolSize_zero_throws()
        {
            var factory = Substitute.For<IGremlinqClientFactory>().Pool();

            factory
                .Invoking(f => f.WithPoolSize(0))
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WithPoolSize_negative_throws()
        {
            var factory = Substitute.For<IGremlinqClientFactory>().Pool();

            factory
                .Invoking(f => f.WithPoolSize(-1))
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WithPoolSize_nine_throws()
        {
            var factory = Substitute.For<IGremlinqClientFactory>().Pool();

            factory
                .Invoking(f => f.WithPoolSize(9))
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WithPoolSize_valid_values()
        {
            var factory = Substitute.For<IGremlinqClientFactory>().Pool();

            for (var i = 1; i <= 8; i++)
            {
                factory
                    .Invoking(f => f.WithPoolSize(i))
                    .Should()
                    .NotThrow();
            }
        }

        [Fact]
        public void WithMaxInProcessPerConnection_zero_throws()
        {
            var factory = Substitute.For<IGremlinqClientFactory>().Pool();

            factory
                .Invoking(f => f.WithMaxInProcessPerConnection(0))
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WithMaxInProcessPerConnection_negative_throws()
        {
            var factory = Substitute.For<IGremlinqClientFactory>().Pool();

            factory
                .Invoking(f => f.WithMaxInProcessPerConnection(-1))
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WithMaxInProcessPerConnection_65_throws()
        {
            var factory = Substitute.For<IGremlinqClientFactory>().Pool();

            factory
                .Invoking(f => f.WithMaxInProcessPerConnection(65))
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WithMaxInProcessPerConnection_valid_values()
        {
            var factory = Substitute.For<IGremlinqClientFactory>().Pool();

            factory
                .Invoking(f => f.WithMaxInProcessPerConnection(1))
                .Should()
                .NotThrow();

            factory
                .Invoking(f => f.WithMaxInProcessPerConnection(64))
                .Should()
                .NotThrow();
        }
    }
}
