using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Assumptions
    {
        [Fact]
        public async Task SemaphoreSlim_WaitAsync_fails_upon_cancellation()
        {
            var semaphore = new SemaphoreSlim(0);
            var cts = new CancellationTokenSource();

            var waitTask = semaphore.WaitAsync(cts.Token);
            await Task.Delay(100);

            waitTask.IsCompleted
                .Should()
                .BeFalse();

            cts.Cancel();

            await waitTask
                .Awaiting(_ => _)
                .Should()
                .ThrowAsync<OperationCanceledException>();
        }
    }
}
