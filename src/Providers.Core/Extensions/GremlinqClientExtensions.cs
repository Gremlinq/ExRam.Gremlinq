using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// Provides extension methods for <see cref="IGremlinqClient"/>.
    /// </summary>
    public static class GremlinqClientExtensions
    {
        private sealed class RequestInterceptingGremlinqClient : IGremlinqClient
        {
            private readonly IGremlinqClient _baseClient;
            private readonly Func<RequestMessage, CancellationToken, Task<RequestMessage>> _transformation;

            public RequestInterceptingGremlinqClient(IGremlinqClient baseClient, Func<RequestMessage, CancellationToken, Task<RequestMessage>> transformation)
            {
                _baseClient = baseClient;
                _transformation = transformation;
            }

            public IAsyncEnumerable<ResponseMessage<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage)
            {
                return Core(requestMessage, this);

                static async IAsyncEnumerable<ResponseMessage<TResult>> Core(RequestMessage requestMessage, RequestInterceptingGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    await foreach (var item in @this._baseClient.SubmitAsync<TResult>(await @this._transformation(requestMessage, ct).ConfigureAwait(false)).WithCancellation(ct).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }

            public void Dispose() => _baseClient.Dispose();
        }

        private sealed class ObserveResultStatusAttributesGremlinqClient : IGremlinqClient
        {
            private readonly IGremlinqClient _baseClient;
            private readonly Action<RequestMessage, IReadOnlyDictionary<string, object>> _observer;

            public ObserveResultStatusAttributesGremlinqClient(IGremlinqClient baseClient, Action<RequestMessage, IReadOnlyDictionary<string, object>> observer)
            {
                _observer = observer;
                _baseClient = baseClient;
            }

            public IAsyncEnumerable<ResponseMessage<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage)
            {
                return Core(requestMessage, this);

                static async IAsyncEnumerable<ResponseMessage<TResult>> Core(RequestMessage requestMessage, ObserveResultStatusAttributesGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    await foreach (var responseMessage in @this._baseClient.SubmitAsync<TResult>(requestMessage).WithCancellation(ct).ConfigureAwait(false))
                    {
                        @this._observer(requestMessage, responseMessage.Status.Attributes);

                        yield return responseMessage;
                    }
                }
            }

            public void Dispose() => _baseClient.Dispose();
        }

        private sealed class LoggingGremlinqClient : IGremlinqClient
        {
            private readonly IGremlinqClient _client;
            private readonly Action<RequestMessage> _logger;

            public LoggingGremlinqClient(IGremlinqClient client, IGremlinQueryEnvironment environment)
            {
                _client = client;
                _logger = GetLoggingFunction(environment);
            }

            public IAsyncEnumerable<ResponseMessage<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage)
            {
                return Core(requestMessage, this);

                static async IAsyncEnumerable<ResponseMessage<TResult>> Core(RequestMessage requestMessage, LoggingGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    var enumerable = @this._client
                        .SubmitAsync<TResult>(requestMessage);

#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                    await using (var e = enumerable.WithCancellation(ct).ConfigureAwait(false).GetAsyncEnumerator())
#pragma warning restore CA2007
                    {
                        var moveNext = e.MoveNextAsync();

                        @this._logger(requestMessage);

                        while (await moveNext)
                        {
                            yield return e.Current;

                            moveNext = e.MoveNextAsync();
                        }
                    }
                }
            }

            private static Action<RequestMessage> GetLoggingFunction(IGremlinQueryEnvironment environment)
            {
                var logLevel = environment.Options.GetValue(GremlinqOption.QueryLogLogLevel);
                var includeBindings = (environment.Options.GetValue(GremlinqOption.QueryLogVerbosity) & QueryLogVerbosity.IncludeBindings) > QueryLogVerbosity.QueryOnly;

                return (requestMessage) =>
                {
                    if (environment.Logger.IsEnabled(logLevel))
                    {
                        if (requestMessage.TryGetGroovyScript(environment, includeBindings) is { } groovyQuery)
                        {
                            if (includeBindings && groovyQuery.Bindings is { } bindings)
                                environment.Logger.LogQuery(logLevel, requestMessage.RequestId, groovyQuery.Script, bindings);
                            else
                                environment.Logger.LogQuery(logLevel, requestMessage.RequestId, groovyQuery.Script);
                        }
                        else
                            environment.Logger.LogQuery(logLevel, requestMessage.RequestId);
                    }
                };
            }

            public void Dispose() => _client.Dispose();
        }

        private sealed class ThrottledGremlinqClient : DisposableBase, IGremlinqClient
        {
            private readonly SemaphoreSlim _semaphore;
            private readonly IGremlinqClient _baseClient;
            private readonly CancellationTokenSource _cts = new ();

            public ThrottledGremlinqClient(IGremlinqClient baseClient, int maxConcurrency)
            {
                _baseClient = baseClient;
                _semaphore = new SemaphoreSlim(maxConcurrency);
            }

            public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
            {
                return Core(message, this);

                static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, ThrottledGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, @this._cts.Token))
                    {
                        await @this._semaphore.WaitAsync(linkedCts.Token).ConfigureAwait(false);

                        try
                        {
                            await foreach (var item in @this._baseClient.SubmitAsync<T>(message).WithCancellation(linkedCts.Token).ConfigureAwait(false))
                            {
                                yield return item;
                            }
                        }
                        finally
                        {
                            @this._semaphore.Release();
                        }
                    }
                }
            }

            protected override void DisposeImpl()
            {
                using (_cts)
                {
                    using (_baseClient)
                    {
                        _cts.Cancel();
                    }
                }
            }
        }

        private sealed class RetryGremlinqClient : IGremlinqClient
        {
            private readonly IGremlinqClient _innerClient;
            private readonly Func<int, Exception, bool> _shouldRetry;

            public RetryGremlinqClient(IGremlinqClient innerClient, Func<int, Exception, bool> shouldRetry)
            {
                _innerClient = innerClient;
                _shouldRetry = shouldRetry;
            }

            public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
            {
                return Core(this, message);

                static async IAsyncEnumerable<ResponseMessage<T>> Core(RetryGremlinqClient @this, RequestMessage message, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    var retry = true;
                    var retryIndex = -1;

                    while (true)
                    {
                        retryIndex++;

#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                        await using (var e = @this._innerClient.SubmitAsync<T>(message).WithCancellation(ct).ConfigureAwait(false).GetAsyncEnumerator())
#pragma warning restore CA2007
                        {
                            while (true)
                            {
                                try
                                {
                                    if (!await e.MoveNextAsync())
                                        yield break;

                                    retry = false;
                                }
                                catch (Exception ex)
                                {
                                    if (retry && @this._shouldRetry(retryIndex, ex))
                                        break;

                                    throw;
                                }

                                yield return e.Current;
                            }
                        }
                    }
                }
            }

            public void Dispose() => _innerClient.Dispose();
        }

        /// <summary>
        /// Wraps the client to intercept and transform request messages before they are submitted.
        /// </summary>
        /// <param name="client">The client to wrap.</param>
        /// <param name="transformation">A function that transforms request messages.</param>
        public static IGremlinqClient TransformRequest(this IGremlinqClient client, Func<RequestMessage, CancellationToken, Task<RequestMessage>> transformation)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(transformation);

            return new RequestInterceptingGremlinqClient(client, transformation);
        }

        /// <summary>
        /// Wraps the client to observe response status attributes for each response message.
        /// </summary>
        /// <param name="client">The client to wrap.</param>
        /// <param name="observer">An action that is called with the request message and the response status attributes.</param>
        public static IGremlinqClient ObserveResultStatusAttributes(this IGremlinqClient client, Action<RequestMessage, IReadOnlyDictionary<string, object>> observer)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(observer);

            return new ObserveResultStatusAttributesGremlinqClient(client, observer);
        }

        /// <summary>
        /// Wraps the client to limit the maximum number of concurrent requests.
        /// </summary>
        /// <param name="client">The client to wrap.</param>
        /// <param name="maxConcurrency">The maximum number of concurrent requests.</param>
        public static IGremlinqClient Throttle(this IGremlinqClient client, int maxConcurrency)
        {
            ArgumentNullException.ThrowIfNull(client);

            return new ThrottledGremlinqClient(client, maxConcurrency);
        }

        internal static IGremlinqClient Log(this IGremlinqClient client, IGremlinQueryEnvironment environment) => new LoggingGremlinqClient(client, environment);

        internal static IGremlinqClient Retry(this IGremlinqClient client, Func<int, Exception, bool> shouldRetry) => new RetryGremlinqClient(client, shouldRetry);
    }
}
