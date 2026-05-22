using FluentAssertions;

using NSubstitute;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class ClientWrapperTests
    {
        [Fact]
        public void TransformRequest_dispose_propagates()
        {
            var inner = Substitute.For<IGremlinqClient>();

            var wrapped = inner.TransformRequest((msg, ct) => Task.FromResult(msg));
            wrapped.Dispose();

            inner.Received(1).Dispose();
        }

        [Fact]
        public void ObserveResultStatusAttributes_dispose_propagates()
        {
            var inner = Substitute.For<IGremlinqClient>();

            var wrapped = inner.ObserveResultStatusAttributes((_, _) => { });
            wrapped.Dispose();

            inner.Received(1).Dispose();
        }

        [Fact]
        public void Throttle_dispose_propagates()
        {
            var inner = Substitute.For<IGremlinqClient>();

            var wrapped = inner.Throttle(4);
            wrapped.Dispose();

            inner.Received(1).Dispose();
        }

        [Fact]
        public void TransformRequest_null_client_throws()
        {
            FluentActions.Invoking(() => GremlinqClientExtensions.TransformRequest(null!, (msg, ct) => Task.FromResult(msg)))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void TransformRequest_null_transformation_throws()
        {
            var inner = Substitute.For<IGremlinqClient>();

            FluentActions.Invoking(() => inner.TransformRequest(null!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void ObserveResultStatusAttributes_null_client_throws()
        {
            FluentActions.Invoking(() => GremlinqClientExtensions.ObserveResultStatusAttributes(null!, (_, _) => { }))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void ObserveResultStatusAttributes_null_observer_throws()
        {
            var inner = Substitute.For<IGremlinqClient>();

            FluentActions.Invoking(() => inner.ObserveResultStatusAttributes(null!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Throttle_null_client_throws()
        {
            FluentActions.Invoking(() => GremlinqClientExtensions.Throttle(null!, 4))
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}
