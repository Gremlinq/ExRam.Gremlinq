using System.Linq.Async;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core.Execution
{
    public static class GremlinQueryExecutor
    {
        private sealed class InvalidGremlinQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context) => throw new InvalidOperationException($"'{nameof(IGremlinQueryExecutor.Execute)}' must not be called on {nameof(GremlinQueryExecutor)}.{nameof(Invalid)}. If you are getting this exception while executing a query, set a proper {nameof(GremlinQueryExecutor)} on the {nameof(GremlinQuerySource)} (e.g. with 'g.UseGremlinServer(...)' for GremlinServer which can be found in the 'ExRam.Gremlinq.Providers.GremlinServer' package).");
        }

        private sealed class EmptyGremlinQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context) => AsyncEnumerable.Empty<T>();
        }

        private sealed class TransformQueryGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<IGremlinQueryBase, IGremlinQueryBase> _transformation;

            public TransformQueryGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor, Func<IGremlinQueryBase, IGremlinQueryBase> transformation)
            {
                _transformation = transformation;
                _baseExecutor = baseExecutor;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context) => _baseExecutor.Execute<T>(context.TransformQuery(_transformation));
        }

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
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<T> Core(CancellationToken ct)
                {
                    var hasSeenFirst = false;
                    var environment = context.Query
                        .AsAdmin().Environment;

                    for (var i = 1; i < int.MaxValue; i++)
                    {
                        await using (var enumerator = _baseExecutor.Execute<T>(context).GetAsyncEnumerator(ct))
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

                                    if (!_shouldRetry(i, ex))
                                        throw;

                                    var waitInterval = TimeSpan.FromMilliseconds(93.75 * Math.Pow(2, Math.Min(i - 1, 5)) + Rnd.Next(8) * 2);
                                    var waitTask = Task.Delay(waitInterval, ct);

                                    var newContext = context.WithNewExecutionId();
                                    environment.Logger.LogInformation($"Query {context.ExecutionId} failed. Backing off for {waitInterval.Milliseconds} milliseconds. It will be retried with new {nameof(GremlinQueryExecutionContext.ExecutionId)} {newContext.ExecutionId}.");

                                    await waitTask;

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

        private sealed class TransformExecutionExceptionGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<GremlinQueryExecutionException, GremlinQueryExecutionException> _exceptionTransformation;

            public TransformExecutionExceptionGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor, Func<GremlinQueryExecutionException, GremlinQueryExecutionException> exceptionTransformation)
            {
                _baseExecutor = baseExecutor;
                _exceptionTransformation = exceptionTransformation;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<T> Core(CancellationToken ct)
                {
                    var enumerator = _baseExecutor
                        .Execute<T>(context)
                        .WithCancellation(ct)
                        .GetAsyncEnumerator();

                    await using (enumerator)
                    {
                        while (true)
                        {
                            try
                            {
                                if (!await enumerator.MoveNextAsync())
                                    yield break;
                            }
                            catch (GremlinQueryExecutionException ex)
                            {
                                throw _exceptionTransformation(ex);
                            }

                            yield return enumerator.Current;
                        }
                    }
                }
            }
        }

        public static readonly IGremlinQueryExecutor Empty = new EmptyGremlinQueryExecutor();

        public static readonly IGremlinQueryExecutor Invalid = new InvalidGremlinQueryExecutor();

        public static IGremlinQueryExecutor TransformQuery(this IGremlinQueryExecutor baseExecutor, Func<IGremlinQueryBase, IGremlinQueryBase> transformation) => new TransformQueryGremlinQueryExecutor(baseExecutor, transformation);

        public static IGremlinQueryExecutor TransformExecutionException(this IGremlinQueryExecutor executor, Func<GremlinQueryExecutionException, GremlinQueryExecutionException> exceptionTransformation) => new TransformExecutionExceptionGremlinQueryExecutor(executor, exceptionTransformation);

        public static IGremlinQueryExecutor RetryWithExponentialBackoff(this IGremlinQueryExecutor executor, Func<int, GremlinQueryExecutionException, bool> shouldRetry) => new ExponentialBackoffExecutor(executor, shouldRetry);
    }
}
