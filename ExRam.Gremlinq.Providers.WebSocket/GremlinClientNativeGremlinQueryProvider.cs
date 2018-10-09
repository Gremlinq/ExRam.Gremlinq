using System;
using System.Collections.Generic;
using System.Linq;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public class GremlinClientNativeGremlinQueryProvider : INativeGremlinQueryProvider<JToken>, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IGremlinClient _gremlinClient;

        public GremlinClientNativeGremlinQueryProvider(IGremlinClient gremlinClient, ILogger logger)
        {
            _logger = logger;
            _gremlinClient = gremlinClient;
        }

        public IAsyncEnumerable<JToken> Execute(string query, IDictionary<string, object> parameters)
        {
            _logger.LogTrace("Executing Gremlin query {0}.", query);

            return _gremlinClient
                .SubmitAsync<JToken>(query, new Dictionary<string, object>(parameters))
                .ToAsyncEnumerable()
                .SelectMany(x => x.ToAsyncEnumerable());
        }

        public void Dispose()
        {
            _gremlinClient.Dispose();
        }
    }
}
