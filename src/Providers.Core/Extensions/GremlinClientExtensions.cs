using System.Runtime.CompilerServices;
using System.Text.Json;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver.Messages;

using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinClientExtensions
    {
        private sealed class RequestInterceptingGremlinClient : IGremlinqClient
        {
            private readonly IGremlinqClient _baseClient;
            private readonly Func<RequestMessage, Task<RequestMessage>> _transformation;

            public RequestInterceptingGremlinClient(IGremlinqClient baseClient, Func<RequestMessage, Task<RequestMessage>> transformation)//TODO: CancellationToken
            {
                _baseClient = baseClient;
                _transformation = transformation;
            }

            public IAsyncEnumerable<ResponseMessage<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage)
            {
                return Core(requestMessage, this);

                static async IAsyncEnumerable<ResponseMessage<TResult>> Core(RequestMessage requestMessage, RequestInterceptingGremlinClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    await foreach(var item in @this._baseClient.SubmitAsync<TResult>(await @this._transformation(requestMessage)))
                    {
                        yield return item;
                    }
                }
            }

            public void Dispose() => _baseClient.Dispose();
        }

        private sealed class ObserveResultStatusAttributesGremlinClient : IGremlinqClient
        {
            private readonly IGremlinqClient _baseClient;
            private readonly Action<RequestMessage, IReadOnlyDictionary<string, object>> _observer;

            public ObserveResultStatusAttributesGremlinClient(IGremlinqClient baseClient, Action<RequestMessage, IReadOnlyDictionary<string, object>> observer)
            {
                _observer = observer;
                _baseClient = baseClient;
            }

            public IAsyncEnumerable<ResponseMessage<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage)
            {
                return Core(requestMessage, this);

                static async IAsyncEnumerable<ResponseMessage<TResult>> Core(RequestMessage requestMessage, ObserveResultStatusAttributesGremlinClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    await foreach (var responseMessage in @this._baseClient.SubmitAsync<TResult>(requestMessage))
                    {
                        @this._observer(requestMessage, responseMessage.Status.Attributes);

                        yield return responseMessage;
                    }
                }
            }

            public void Dispose() => _baseClient.Dispose();
        }

        private sealed class LoggingGremlinQueryClient : IGremlinqClient
        {
            private static readonly JsonSerializerOptions IndentedSerializerOptions = new() { WriteIndented = true };
            private static readonly JsonSerializerOptions NotIndentedSerializerOptions = new() { WriteIndented = false };

            private readonly IGremlinqClient _client;
            private readonly Action<RequestMessage> _logger;
            private readonly IGremlinQueryEnvironment _environment;

            public LoggingGremlinQueryClient(IGremlinqClient client, IGremlinQueryEnvironment environment)
            {
                _client = client;
                _environment = environment;
                _logger = GetLoggingFunction(environment);
            }

            public IAsyncEnumerable<ResponseMessage<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage)
            {
                return Core(requestMessage, this);

                static async IAsyncEnumerable<ResponseMessage<TResult>> Core(RequestMessage requestMessage, LoggingGremlinQueryClient @this, [EnumeratorCancellation] CancellationToken ct = default)
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

        public static IGremlinqClient TransformRequest(this IGremlinqClient client, Func<RequestMessage, Task<RequestMessage>> transformation) => new RequestInterceptingGremlinClient(client, transformation);

        public static IGremlinqClient ObserveResultStatusAttributes(this IGremlinqClient client, Action<RequestMessage, IReadOnlyDictionary<string, object>> observer) => new ObserveResultStatusAttributesGremlinClient(client, observer);

        internal static IGremlinqClient Log(this IGremlinqClient client, IGremlinQueryEnvironment environment) => new LoggingGremlinQueryClient(client, environment);
    }
}
