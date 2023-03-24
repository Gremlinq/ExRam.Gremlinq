using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Process.Traversal;

using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core.Execution
{
    public static class GremlinQueryExecutorExtensions
    {
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

            public IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    var hasSeenFirst = false;

                    for (var i = 0; i < int.MaxValue; i++)
                    {
                        await using (var enumerator = _baseExecutor.Execute(query, environment).GetAsyncEnumerator(ct))
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

            public IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    IAsyncEnumerator<object> enumerator;

                    try
                    {
                        enumerator = _baseExecutor
                            .Execute(query, environment)
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

        public static IGremlinQueryExecutor TransformExecutionException(this IGremlinQueryExecutor executor, Func<Exception, Exception> exceptionTransformation) => new TransformExecutionExceptionGremlinQueryExecutor(executor, exceptionTransformation);

        public static IGremlinQueryExecutor RetryWithExponentialBackoff(this IGremlinQueryExecutor executor, Func<int, ResponseException, bool> shouldRetry) => new ExponentialBackoffExecutor(executor, shouldRetry);
    }
}
