using System;
using System.Collections.Generic;
using System.Linq;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public class ClientGremlinQueryProvider : IGremlinQueryProvider, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IGremlinClient _gremlinClient;

        public ClientGremlinQueryProvider(IGremlinClient client) : this(client, NullLogger.Instance)
        {

        }

        public ClientGremlinQueryProvider(IGremlinClient client, ILogger logger)
        {
            _logger = logger;
            _gremlinClient = client;
        }

        public void Dispose()
        {
            _gremlinClient.Dispose();
        }

        public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
        {
            if (typeof(TElement) != typeof(JToken))
                throw new NotSupportedException();

            var serialized = query
                .Serialize();

            _logger.LogTrace("Executing Gremlin query {0}.", serialized.queryString);

            return _gremlinClient
                .SubmitAsync<JToken>(serialized.queryString, new Dictionary<string, object>(serialized.parameters))
                .ToAsyncEnumerable()
                .SelectMany(x => x
                    .ToAsyncEnumerable()
                    .Select(y => (TElement)(object)y));
        }
    }
}
