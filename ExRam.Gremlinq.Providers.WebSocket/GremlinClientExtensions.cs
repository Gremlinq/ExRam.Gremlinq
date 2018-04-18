using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class GremlinClientExtensions
    {
        private sealed class GremlinClientNativeGremlinQueryProvider : INativeGremlinQueryProvider<JToken>, IDisposable
        {
            private readonly ILogger _logger;
            private readonly IGremlinClient _gremlinClient;

            public GremlinClientNativeGremlinQueryProvider(IGremlinClient gremlinClient, string traversalSourceName, ILogger logger)
            {
                this._logger = logger;
                this._gremlinClient = gremlinClient;
                this.TraversalSource = GremlinQuery.Create(traversalSourceName);
            }

            public IAsyncEnumerable<JToken> Execute(string query, IDictionary<string, object> parameters)
            {
                this._logger.LogTrace("Executing Gremlin query {0}.", query);

                return this._gremlinClient
                    .SubmitAsync<JToken>(query, new Dictionary<string, object>(parameters))
                    .ToAsyncEnumerable()
                    .SelectMany(x => x.ToAsyncEnumerable());
            }

            public void Dispose()
            {
                this._gremlinClient.Dispose();
            }

            public IGremlinQuery<Unit> TraversalSource { get; }
        }

        public static INativeGremlinQueryProvider<JToken> ToNativeGremlinQueryProvider(this IGremlinClient client, string traversalSourceName, ILogger logger)
        {
            return new GremlinClientNativeGremlinQueryProvider(client, traversalSourceName, logger);
        }
    }
}
