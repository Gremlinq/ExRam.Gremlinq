using System;
using System.Collections.Generic;
using System.Linq;
using ExRam.Gremlinq.Serialization;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public class ClientGremlinQueryProvider : IGremlinQueryProvider, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IGremlinClient _gremlinClient;
        private readonly IGremlinQuerySerializer<(string queryString, IDictionary<string, object> parameters)> _serializer;

        public ClientGremlinQueryProvider(IGremlinClient client, IGremlinQuerySerializer<(string queryString, IDictionary<string, object> parameters)> serializer, ILogger logger = null)
        {
            _logger = logger;
            _gremlinClient = client;
            _serializer = serializer;
        }

        public void Dispose()
        {
            _gremlinClient.Dispose();
        }

        public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
        {
            if (typeof(TElement) != typeof(JToken))
                throw new NotSupportedException();

            var serialized = _serializer.Serialize(query);

            _logger?.LogTrace("Executing Gremlin query {0}.", serialized.queryString);

            return _gremlinClient
                .SubmitAsync<JToken>(serialized.queryString, new Dictionary<string, object>(serialized.parameters))
                .ToAsyncEnumerable()
                .SelectMany(x => x
                    .ToAsyncEnumerable()
                    .Select(y => (TElement)(object)y));
        }
    }
}
