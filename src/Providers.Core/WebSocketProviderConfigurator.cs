using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;

using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Driver.Messages;
using static Gremlin.Net.Driver.Messages.ResponseStatusCode;

namespace ExRam.Gremlinq.Providers.Core
{
    public sealed class WebSocketProviderConfigurator : IWebSocketProviderConfigurator<WebSocketProviderConfigurator>
    {
        private sealed class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinqClientFactory _clientFactory;
            private readonly ConcurrentDictionary<IGremlinQueryEnvironment, IGremlinqClient> _clients = new();

            public WebSocketGremlinQueryExecutor(IGremlinqClientFactory clientFactory)
            {
                _clientFactory = clientFactory;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                return Core(this, context);

                async static IAsyncEnumerable<T> Core(WebSocketGremlinQueryExecutor @this, GremlinQueryExecutionContext context, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    var environment = context.Query
                        .AsAdmin()
                        .Environment;

                    var client = @this._clients.GetOrAdd(
                        environment,
                        static (environment, executor) => executor._clientFactory.Create(environment),
                        @this);

                    var requestMessage = environment
                        .Serializer
                        .TransformTo<RequestMessage>()
                        .From(context.Query, environment)
                        .Rebuild()
                        .OverrideRequestId(context.ExecutionId)
                        .Create();

                    ResponseMessage<List<T>>? maybeResults;

                    try
                    {
                        maybeResults = await client
                            .SendAsync<List<T>>(requestMessage, ct)
                            .ConfigureAwait(false);

                        if (maybeResults is { Status: { Code: { } code } status } && code is not Success and not NoContent and not PartialContent and not Authenticate)
                            throw new ResponseException(code, status.Attributes, $"{status.Code}: {status.Message}");
                    }
                    catch (ConnectionClosedException ex)
                    {
                        throw new GremlinQueryExecutionException(context, ex);
                    }
                    catch (NoConnectionAvailableException ex)
                    {
                        throw new GremlinQueryExecutionException(context, ex);
                    }
                    catch (ResponseException ex)
                    {
                        throw new GremlinQueryExecutionException(context, ex);
                    }

                    if (maybeResults is { Result.Data: { } data })
                    {
                        foreach (var obj in data)
                        {
                            yield return obj;
                        }
                    }
                }
            }
        }

        public static readonly WebSocketProviderConfigurator Default = new (GremlinqConfigurator.Identity, GremlinClientFactory.LocalHost);

        private readonly IGremlinqClientFactory _clientFactory;
        private readonly GremlinqConfigurator _gremlinqConfigurator;

        private WebSocketProviderConfigurator(
            GremlinqConfigurator gremlinqConfigurator,
            IGremlinqClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _gremlinqConfigurator = gremlinqConfigurator;
        }

        public WebSocketProviderConfigurator ConfigureClientFactory(Func<IGremlinqClientFactory, IGremlinqClientFactory> transformation) => new (
            _gremlinqConfigurator,
            transformation(_clientFactory));

        public WebSocketProviderConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new(
            _gremlinqConfigurator.ConfigureQuerySource(transformation),
            _clientFactory);

        public IGremlinQuerySource Transform(IGremlinQuerySource source) => _gremlinqConfigurator
            .Transform(source
                .ConfigureEnvironment(environment => environment
                    .UseExecutor(new WebSocketGremlinQueryExecutor(_clientFactory.Log()))));
    }
}
