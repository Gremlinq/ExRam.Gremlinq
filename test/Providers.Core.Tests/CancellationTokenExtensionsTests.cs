using FluentAssertions;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class CancellationTokenExtensionsTests
    {
        [Fact]
        public void TryLink_links_two_tokens_that_can_be_cancelled()
        {
            using var first = new CancellationTokenSource();
            using var second = new CancellationTokenSource();

            using var linked = first.Token.TryLink(second.Token, out var token);

            linked.Should().NotBeNull();
            token.CanBeCanceled.Should().BeTrue();
            token.IsCancellationRequested.Should().BeFalse();

            second.Cancel();

            token.IsCancellationRequested.Should().BeTrue();
        }

        [Fact]
        public void TryLink_names_the_first_token_when_only_it_can_be_cancelled()
        {
            using var first = new CancellationTokenSource();

            first.Token.TryLink(default, out var token).Should().BeNull();

            token.Should().Be(first.Token);
        }

        [Fact]
        public void TryLink_names_the_second_token_when_only_it_can_be_cancelled()
        {
            using var second = new CancellationTokenSource();

            default(CancellationToken).TryLink(second.Token, out var token).Should().BeNull();

            token.Should().Be(second.Token);
        }

        [Fact]
        public void TryLink_names_a_token_that_cannot_be_cancelled_when_neither_can()
        {
            default(CancellationToken).TryLink(default, out var token).Should().BeNull();

            token.CanBeCanceled.Should().BeFalse();
        }
    }
}
