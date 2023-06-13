using System.Text.Json;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver.Messages;
using Microsoft.Extensions.Logging;

namespace Gremlin.Net.Driver
{
    public static class GremlinClientExtensions
    {
        private sealed class RequestInterceptingGremlinClient : IGremlinClient
        {
            private readonly IGremlinClient _baseClient;
            private readonly Func<RequestMessage, Task<RequestMessage>> _transformation;

            public RequestInterceptingGremlinClient(IGremlinClient baseClient, Func<RequestMessage, Task<RequestMessage>> transformation)
            {
                _baseClient = baseClient;
                _transformation = transformation;
            }

            public async Task<ResultSet<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage, CancellationToken ct)
            {
                return await _baseClient.SubmitAsync<TResult>(await _transformation(requestMessage), ct);
            }

            public void Dispose()
            {
                _baseClient.Dispose();
            }
        }

        private sealed class ObserveResultStatusAttributesGremlinClient : IGremlinClient
        {
            private readonly IGremlinClient _baseClient;
            private readonly Action<RequestMessage, IReadOnlyDictionary<string, object>> _observer;

            public ObserveResultStatusAttributesGremlinClient(IGremlinClient baseClient, Action<RequestMessage, IReadOnlyDictionary<string, object>> observer)
            {
                _observer = observer;
                _baseClient = baseClient;
            }

            public async Task<ResultSet<TResult>> SubmitAsync<TResult>(RequestMessage requestMessage, CancellationToken ct)
            {
                var resultSet = await _baseClient.SubmitAsync<TResult>(requestMessage, ct);

                _observer(requestMessage, resultSet.StatusAttributes);

                return resultSet;
            }

            public void Dispose()
            {
                _baseClient.Dispose();
            }
        }

        private sealed class LoggingGremlinQueryClient : IGremlinClient
        {
            private static readonly JsonSerializerOptions IndentedSerializerOptions = new() { WriteIndented = true };
            private static readonly JsonSerializerOptions NotIndentedSerializerOptions = new() { WriteIndented = false };

            private readonly IGremlinClient _client;
            private readonly Action<RequestMessage> _logger;

            public LoggingGremlinQueryClient(IGremlinClient client, IGremlinQueryEnvironment environment)
            {
                _client = client;
                _logger = GetLoggingFunction(environment);
            }

            public async Task<ResultSet<T>> SubmitAsync<T>(RequestMessage requestMessage, CancellationToken cancellationToken = default)
            {
                var task = _client.SubmitAsync<T>(requestMessage, cancellationToken);

                _logger(requestMessage);

                return await task;
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
                        if (requestMessage.TryGetGroovyQuery(environment, includeBindings) is { } groovyQuery)
                        {
                            environment.Logger.Log(
                                logLevel,
                                "Executing Gremlin query {0}.",
                                JsonSerializer.Serialize(
                                    new
                                    {
                                        requestMessage.RequestId,
                                        groovyQuery.Script,
                                        groovyQuery.Bindings
                                    },
                                    formatting));
                        }
                        else
                            environment.Logger.LogWarning($"Failed to log {nameof(RequestMessage)} {requestMessage.RequestId}.");
                    }
                };
            }

            public void Dispose()
            {
                _client.Dispose();
            }
        }

        public static IGremlinClient TransformRequest(this IGremlinClient client, Func<RequestMessage, Task<RequestMessage>> transformation) => new RequestInterceptingGremlinClient(client, transformation);

        public static IGremlinClient ObserveResultStatusAttributes(this IGremlinClient client, Action<RequestMessage, IReadOnlyDictionary<string, object>> observer) => new ObserveResultStatusAttributesGremlinClient(client, observer);

        internal static IGremlinClient Log(this IGremlinClient client, IGremlinQueryEnvironment environment) => new LoggingGremlinQueryClient(client, environment);
    }
}
