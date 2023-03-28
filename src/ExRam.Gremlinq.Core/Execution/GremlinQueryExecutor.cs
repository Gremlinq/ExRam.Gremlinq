using Gremlin.Net.Driver.Exceptions;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core.Execution
{
    public static class GremlinQueryExecutor
    {
        private sealed class InvalidGremlinQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<T> Execute<T>(IGremlinQueryBase query) => throw new InvalidOperationException($"'{nameof(IGremlinQueryExecutor.Execute)}' must not be called on {nameof(GremlinQueryExecutor)}.{nameof(Invalid)}. If you are getting this exception while executing a query, set a proper {nameof(GremlinQueryExecutor)} on the {nameof(GremlinQuerySource)} (e.g. with 'g.UseGremlinServer(...)' for GremlinServer which can be found in the 'ExRam.Gremlinq.Providers.GremlinServer' package).");
        }

        private sealed class EmptyGremlinQueryExecutor : IGremlinQueryExecutor
        {
            public IAsyncEnumerable<T> Execute<T>(IGremlinQueryBase query) => AsyncEnumerable.Empty<T>();
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

            public IAsyncEnumerable<T> Execute<T>(IGremlinQueryBase query) => _baseExecutor.Execute<T>(_transformation(query));
        }

        private sealed class ExponentialBackoffExecutor : IGremlinQueryExecutor
        {
            [ThreadStatic]
            private static Random? _rnd;

            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<int, ResponseException, bool> _shouldRetry;

            public ExponentialBackoffExecutor(IGremlinQueryExecutor baseExecutor, Func<int, ResponseException, bool> shouldRetry)
            {
                _baseExecutor = baseExecutor;
                _shouldRetry = shouldRetry;
            }

            public IAsyncEnumerable<T> Execute<T>(IGremlinQueryBase query)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<T> Core(CancellationToken ct)
                {
                    var hasSeenFirst = false;
                    var environment = query.AsAdmin().Environment;

                    for (var i = 0; i < int.MaxValue; i++)
                    {
                        await using (var enumerator = _baseExecutor.Execute<T>(query).GetAsyncEnumerator(ct))
                        {
                            while (true)
                            {
                                try
                                {
                                    if (!await enumerator.MoveNextAsync())
                                        yield break;

                                    hasSeenFirst = true;
                                }
                                catch (ResponseException ex)
                                {
                                    environment.Logger.LogInformation(ex, $"Query failed.");

                                    if (hasSeenFirst)
                                        throw;

                                    if (!_shouldRetry(i, ex))
                                        throw;

                                    //This is done not to end up with the same seeds if many of these
                                    //requests fail roughly at the same time
                                    await Task.Delay((_rnd ??= new Random((int)(DateTime.Now.Ticks & int.MaxValue) ^ Thread.CurrentThread.ManagedThreadId)).Next(i + 2) * 16, ct);

                                    environment.Logger.LogInformation($"Retrying query.");

                                    break;
                                }

                                yield return enumerator.Current;
                            }
                        }
                    }
                }
            }
        }

        private sealed class TransformExecutionExceptionGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinQueryExecutor _baseExecutor;
            private readonly Func<Exception, Exception> _exceptionTransformation;

            public TransformExecutionExceptionGremlinQueryExecutor(IGremlinQueryExecutor baseExecutor, Func<Exception, Exception> exceptionTransformation)
            {
                _baseExecutor = baseExecutor;
                _exceptionTransformation = exceptionTransformation;
            }

            public IAsyncEnumerable<T> Execute<T>(IGremlinQueryBase query)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<T> Core(CancellationToken ct)
                {
                    IAsyncEnumerator<T> enumerator;

                    try
                    {
                        enumerator = _baseExecutor
                            .Execute<T>(query)
                            .GetAsyncEnumerator(ct);
                    }
                    catch (Exception ex)
                    {
                        throw _exceptionTransformation(ex);
                    }

                    await using (enumerator)
                    {
                        while (true)
                        {
                            try
                            {
                                if (!await enumerator.MoveNextAsync())
                                    yield break;
                            }
                            catch (Exception ex)
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

        public static IGremlinQueryExecutor TransformExecutionException(this IGremlinQueryExecutor executor, Func<Exception, Exception> exceptionTransformation) => new TransformExecutionExceptionGremlinQueryExecutor(executor, exceptionTransformation);

        public static IGremlinQueryExecutor RetryWithExponentialBackoff(this IGremlinQueryExecutor executor, Func<int, ResponseException, bool> shouldRetry) => new ExponentialBackoffExecutor(executor, shouldRetry);
    }
}
