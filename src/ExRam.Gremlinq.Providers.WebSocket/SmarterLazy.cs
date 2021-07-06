using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    internal sealed class SmarterLazy<T> : IDisposable
    {
        private static readonly TaskCompletionSource<T> DisposedTcs = new();

        private TaskCompletionSource<T>? _tcs;
        private readonly Func<ILogger, Task<T>> _factory;

        static SmarterLazy()
        {
            DisposedTcs.TrySetException(new ObjectDisposedException(nameof(SmarterLazy<T>)));
        }

        public SmarterLazy(Func<ILogger, Task<T>> factory)
        {
            _factory = factory;
        }

        public async Task<T> GetValue(ILogger logger)
        {
            TaskCompletionSource<T>? localTcs = null;

            while (true)
            {
                if (Volatile.Read(ref _tcs) is { } tcs)
                    return await tcs.Task;

                if (localTcs == null)
                    localTcs = new TaskCompletionSource<T>();

                if (Interlocked.CompareExchange(ref _tcs, localTcs, null) == null)
                {
                    try
                    {
                        var ret = await _factory(logger);

                        localTcs.TrySetResult(ret);

                        return ret;
                    }
                    catch (Exception ex)
                    {
                        localTcs.TrySetException(ex);

                        Interlocked.CompareExchange(ref _tcs, null!, localTcs);

                        throw;
                    }
                }
            }
        }

        public void Dispose()
        {
            var maybePrevious = Interlocked.Exchange(ref _tcs, DisposedTcs);

            if (maybePrevious is { } previous && previous != DisposedTcs)
            {
                previous.Task
                    .ContinueWith(async t =>
                    {
                        if (t.Result is IDisposable disposable)
                            disposable.Dispose();
                        else if (t.Result is IAsyncDisposable asyncDisposable)
                            await asyncDisposable.DisposeAsync();
                    });
            }
        }
    }
}
