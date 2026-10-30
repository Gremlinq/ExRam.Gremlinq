using FluentAssertions;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class ReceivedMessageTests
    {
        [Fact]
        public void Memory_is_the_advanced_part()
        {
            using var message = ReceivedMessage.Rent();

            var free = message.GetFreeMemory();
            free.Length.Should().BeGreaterThanOrEqualTo(8192);

            free.Span[0] = 1;
            free.Span[1] = 2;
            message.Advance(2);

            message.Memory.ToArray().Should().Equal(1, 2);
        }

        [Fact]
        public void Free_memory_grows_when_the_array_is_full_and_keeps_the_bytes()
        {
            using var message = ReceivedMessage.Rent();

            var free = message.GetFreeMemory();
            var length = free.Length;

            free.Span.Fill(7);
            message.Advance(length);

            var grown = message.GetFreeMemory();

            grown.Length.Should().BeGreaterThanOrEqualTo(length);
            message.Memory.Length.Should().Be(length);
            message.Memory.Span.ToArray().Should().AllBeEquivalentTo((byte)7);
        }

        [Fact]
        public void Disposed_message_has_no_memory()
        {
            var message = ReceivedMessage.Rent();

            message.Dispose();
            message.Dispose();

            message
                .Invoking(message => message.Memory)
                .Should()
                .Throw<ObjectDisposedException>();

            message
                .Invoking(message => message.GetFreeMemory())
                .Should()
                .Throw<ObjectDisposedException>();
        }
    }
}
