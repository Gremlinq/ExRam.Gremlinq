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
            await Task.Delay(100, TestContext.Current.CancellationToken);

            waitTask.IsCompleted
                .Should()
                .BeFalse();

            cts.Cancel();

            await waitTask
                .Awaiting(_ => _)
                .Should()
                .ThrowAsync<OperationCanceledException>();
        }

        [Fact]
        public async Task SemaphoreSlim_WaitAsync_is_stuck_upon_disposal()
        {
            var semaphore = new SemaphoreSlim(0);
            var cts = new CancellationTokenSource();

            var waitTask = semaphore.WaitAsync(cts.Token);
            await Task.Delay(100, TestContext.Current.CancellationToken);

            waitTask.IsCompleted
                .Should()
                .BeFalse();

            semaphore.Dispose();

            await Task.Delay(100, TestContext.Current.CancellationToken);

            waitTask.IsCompleted
                .Should()
                .BeFalse();

            cts.Cancel();

            await Task.Delay(100, TestContext.Current.CancellationToken);

            waitTask.IsCompleted
                .Should()
                .BeFalse();
        }
    }
}
