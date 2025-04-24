using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core.Execution
{
    public static partial class GremlinQueryExecutor
    {
        private sealed class ExponentialBackoffExecutor : IGremlinQueryExecutor
        {
            [ThreadStatic]
            private static Random? _rnd;

            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<int, GremlinQueryExecutionException, bool> _shouldRetry;

            public ExponentialBackoffExecutor(IGremlinQueryExecutor baseExecutor, Func<int, GremlinQueryExecutionException, bool> shouldRetry)
            {
                _baseExecutor = baseExecutor;
                _shouldRetry = shouldRetry;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                return Core(this, context);

                static async IAsyncEnumerable<T> Core(ExponentialBackoffExecutor @this, GremlinQueryExecutionContext context, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    var hasSeenFirst = false;
                    var environment = context.Query
                        .AsAdmin().Environment;

                    for (var i = 1; i < int.MaxValue; i++)
                    {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                        await using (var enumerator = @this._baseExecutor.Execute<T>(context).WithCancellation(ct).ConfigureAwait(false).GetAsyncEnumerator())
#pragma warning restore CA2007
                        {
                            while (true)
                            {
                                try
                                {
                                    if (!await enumerator.MoveNextAsync())
                                        yield break;

                                    hasSeenFirst = true;
                                }
                                catch (GremlinQueryExecutionException ex)
                                {
                                    if (hasSeenFirst)
                                        throw;

                                    if (!@this._shouldRetry(i, ex))
                                        throw;

                                    var waitInterval = TimeSpan.FromMilliseconds(93.75 * Math.Pow(2, Math.Min(i - 1, 5)) + Rnd.Next(8) * 2);
                                    var waitTask = Task.Delay(waitInterval, ct);

                                    var newContext = context.WithNewExecutionId();
                                    environment.Logger.LogInformation("Query {executionId} failed. Backing off for {waitInterval} milliseconds. It will be retried with new ExecutionId {newExecutionId}.", context.ExecutionId, waitInterval.Milliseconds, newContext.ExecutionId);

                                    await waitTask.ConfigureAwait(false);

                                    context = newContext;

                                    break;
                                }

                                yield return enumerator.Current;
                            }
                        }
                    }
                }
            }

            private static Random Rnd
            {
                get => _rnd ??= new Random((int)(DateTime.Now.Ticks & int.MaxValue) ^ Environment.CurrentManagedThreadId);
            }
        }

        [Obsolete("Will be removed in a future version. Retries should be applied on a per-query basis and is better left to resilience-libraries (e.g. Polly). To recreate the behaviour of this method in your own code, see https://github.com/Gremlinq/ExRam.Gremlinq/blob/12.x/src/Core/Execution/GremlinQueryExecutor%20(Retry).cs.")]
        public static IGremlinQueryExecutor RetryWithExponentialBackoff(this IGremlinQueryExecutor executor, Func<int, GremlinQueryExecutionException, bool> shouldRetry) => new ExponentialBackoffExecutor(executor, shouldRetry);
    }
}
