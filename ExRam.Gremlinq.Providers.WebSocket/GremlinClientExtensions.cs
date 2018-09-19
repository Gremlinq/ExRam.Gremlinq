using System;
using System.Collections.Generic;
using System.Linq;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    // ReSharper disable once UnusedMember.Global
    public static class GremlinClientExtensions
    {
        private sealed class GremlinClientNativeGremlinQueryProvider : INativeGremlinQueryProvider<JToken>, IDisposable
        {
            private readonly ILogger _logger;
            private readonly IGremlinClient _gremlinClient;

            public GremlinClientNativeGremlinQueryProvider(IGremlinClient gremlinClient, ILogger logger)
            {
                this._logger = logger;
                this._gremlinClient = gremlinClient;
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
        }

        public static INativeGremlinQueryProvider<JToken> ToNativeGremlinQueryProvider(this IGremlinClient client, ILogger logger)
        {
            return new GremlinClientNativeGremlinQueryProvider(client, logger);
        }
    }
}
