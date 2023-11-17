using System.Runtime.CompilerServices;
using System.Text.Json;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver.Messages;

using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Providers.Core
{
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
                    await foreach(var item in @this._baseClient.SubmitAsync<TResult>(await @this._transformation(requestMessage, ct)).WithCancellation(ct))
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
                    await foreach (var responseMessage in @this._baseClient.SubmitAsync<TResult>(requestMessage).WithCancellation(ct))
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
            private static readonly JsonSerializerOptions IndentedSerializerOptions = new() { WriteIndented = true };
            private static readonly JsonSerializerOptions NotIndentedSerializerOptions = new() { WriteIndented = false };

            private readonly IGremlinqClient _client;
            private readonly Action<RequestMessage> _logger;
            private readonly IGremlinQueryEnvironment _environment;

            public LoggingGremlinqClient(IGremlinqClient client, IGremlinQueryEnvironment environment)
            {
                _client = client;
                _environment = environment;
                _logger = GetLoggingFunction(environment);
            }

            public IAsyncEnumerable<ResponseMessage<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage)
            {
                return Core(requestMessage, this);

                static async IAsyncEnumerable<ResponseMessage<TResult>> Core(RequestMessage requestMessage, LoggingGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    @this._logger(requestMessage);

                    await using (var e = @this._client.SubmitAsync<TResult>(requestMessage).GetAsyncEnumerator(ct))
                    {
                        while (true)
                        {
                            try
                            {
                                if (!await e.MoveNextAsync())
                                    break;
                            }
                            catch (Exception ex)
                            {
                                @this._environment.Logger.LogError(ex, "Execution of Gremlin query {RequestId} failed.", requestMessage.RequestId);

                                throw;
                            }

                            yield return e.Current;
                        }   
                    }
                }
            }

            private static Action<RequestMessage> GetLoggingFunction(IGremlinQueryEnvironment environment)
            {
                var logLevel = environment.Options.GetValue(GremlinqOption.QueryLogLogLevel);
                var includeBindings = (environment.Options.GetValue(GremlinqOption.QueryLogVerbosity) & QueryLogVerbosity.IncludeBindings) > QueryLogVerbosity.QueryOnly;
                var formatting = environment.Options.GetValue(GremlinqOption.QueryLogFormatting).HasFlag(QueryLogFormatting.Indented)
                    ? IndentedSerializerOptions
                    : NotIndentedSerializerOptions;

                return (requestMessage) =>
                {
                    if (environment.Logger.IsEnabled(logLevel))
                    {
                        if (requestMessage.TryGetGroovyScript(environment, includeBindings) is { } groovyQuery)
                        {
                            environment.Logger.Log(
                                logLevel,
                                "Executing Gremlin query {RequestId} (Script={Script}, Bindings={Bindings}).",
                                requestMessage.RequestId,
                                groovyQuery.Script,
                                JsonSerializer.Serialize(groovyQuery.Bindings, formatting));
                        }
                        else
                            environment.Logger.LogWarning("Failed to log RequestMessage {RequestId}.", requestMessage.RequestId);
                    }
                };
            }

            public void Dispose() => _client.Dispose();
        }

        private sealed class ThrottledGremlinqClient : IGremlinqClient
        {
            private readonly SemaphoreSlim _semaphore;
            private readonly IGremlinqClient _baseClient;
            private readonly CancellationTokenSource _cts = new ();

            public ThrottledGremlinqClient(IGremlinqClient baseClient, int maxConcurrency)
            {
                _baseClient = baseClient;
                _semaphore = new SemaphoreSlim(maxConcurrency);
            }

            public void Dispose()
            {
                using (_cts)
                {
                    using (_baseClient)
                    {
                        _cts.Cancel();
                    }
                }
            }

            public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
            {
                return Core(message, this);

                static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, ThrottledGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, @this._cts.Token);

                    await @this._semaphore.WaitAsync(linkedCts.Token);

                    try
                    {
                        await foreach (var item in @this._baseClient.SubmitAsync<T>(message).WithCancellation(linkedCts.Token))
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

        public static IGremlinqClient TransformRequest(this IGremlinqClient client, Func<RequestMessage, CancellationToken, Task<RequestMessage>> transformation) => new RequestInterceptingGremlinqClient(client, transformation);

        public static IGremlinqClient ObserveResultStatusAttributes(this IGremlinqClient client, Action<RequestMessage, IReadOnlyDictionary<string, object>> observer) => new ObserveResultStatusAttributesGremlinqClient(client, observer);

        public static IGremlinqClient Throttle(this IGremlinqClient client, int maxConcurrency) => new ThrottledGremlinqClient(client, maxConcurrency);

        internal static IGremlinqClient Log(this IGremlinqClient client, IGremlinQueryEnvironment environment) => new LoggingGremlinqClient(client, environment);
    }
}
