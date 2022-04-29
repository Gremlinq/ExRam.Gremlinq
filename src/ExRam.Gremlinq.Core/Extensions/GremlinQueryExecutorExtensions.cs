using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Driver.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

            public IAsyncEnumerable<object> Execute(ISerializedGremlinQuery serializedQuery, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    var hasSeenFirst = false;

                    for (var i = 0; i < int.MaxValue; i++)
                    {
                        await using (var enumerator = _baseExecutor.Execute(serializedQuery, environment).GetAsyncEnumerator(ct))
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
                                    if (hasSeenFirst)
                                        throw;

                                    if (!_shouldRetry(i, ex))
                                        throw;

                                    //This is done not to end up with the same seeds if many of these
                                    //requests fail roughly at the same time
                                    await Task.Delay((_rnd ??= new Random((int)(DateTime.Now.Ticks & int.MaxValue) ^ Thread.CurrentThread.ManagedThreadId)).Next(i + 2) * 16, ct);

                                    var newSerializedQuery = serializedQuery.WithNewId();
                                    environment.Logger.LogInformation($"Retrying serialized query {serializedQuery.Id} with new {nameof(ISerializedGremlinQuery.Id)} {newSerializedQuery.Id}.");
                                    serializedQuery = newSerializedQuery;

                                    break;
                                }

                                yield return enumerator.Current;
                            }
                        }
                    }
                }
            }
        }

        private sealed class LoggingGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, Action<ISerializedGremlinQuery, string>> Loggers = new();

            private readonly IGremlinQueryExecutor _executor;

            public LoggingGremlinQueryExecutor(IGremlinQueryExecutor executor)
            {
                _executor = executor;
            }

            public IAsyncEnumerable<object> Execute(ISerializedGremlinQuery serializedQuery, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    await using (var enumerator = _executor.Execute(serializedQuery, environment).GetAsyncEnumerator(ct))
                    {
                        try
                        {
                            var moveNext = enumerator.MoveNextAsync();

                            var logger = Loggers.GetValue(
                                environment,
                                static environment => GetLoggingFunction(environment));

                            logger(serializedQuery, serializedQuery.Id);

                            if (!await moveNext)
                                yield break;
                        }
                        catch (Exception ex)
                        {
                            environment.Logger.LogError($"Error executing Gremlin query with id {serializedQuery.Id}: {ex}");

                            throw;
                        }

                        do
                        {
                            yield return enumerator.Current;
                        } while (await enumerator.MoveNextAsync());
                    }
                }
            }

            private static Action<ISerializedGremlinQuery, string> GetLoggingFunction(IGremlinQueryEnvironment environment)
            {
                var logLevel = environment.Options.GetValue(GremlinqOption.QueryLogLogLevel);
                var verbosity = environment.Options.GetValue(GremlinqOption.QueryLogVerbosity);
                var formatting = environment.Options.GetValue(GremlinqOption.QueryLogFormatting);

                return (serializedQuery, requestId) =>
                {
                    if (environment.Logger.IsEnabled(logLevel))
                    {
                        var groovyQuery = serializedQuery.ToGroovy();

                        environment.Logger.Log(
                            logLevel,
                            "Executing Gremlin query {0}.",
                            JsonConvert.SerializeObject(
                                new
                                {
                                    RequestId = requestId,
                                    groovyQuery.Script,
                                    Bindings = (verbosity & QueryLogVerbosity.IncludeBindings) > QueryLogVerbosity.QueryOnly
                                        ? groovyQuery.Bindings
                                        : null
                                },
                                formatting));
                    }
                };
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

            public IAsyncEnumerable<object> Execute(ISerializedGremlinQuery serializedQuery, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    IAsyncEnumerator<object> enumerator;

                    try
                    {
                        enumerator = _baseExecutor
                            .Execute(serializedQuery, environment)
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

        public static IGremlinQueryExecutor Log(this IGremlinQueryExecutor executor) => new LoggingGremlinQueryExecutor(executor);
    }
}
