using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Serialization;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
    {
        private static readonly ConditionalWeakTable<IGraphModel, GraphsonDeserializer> Serializers = new ConditionalWeakTable<IGraphModel, GraphsonDeserializer>();

        private readonly ILogger _logger;
        private readonly IGremlinClient _gremlinClient;
        private readonly IGremlinQuerySerializer<(string queryString, IDictionary<string, object> parameters)> _serializer;

        public WebSocketGremlinQueryExecutor(IGremlinClient client, IGremlinQuerySerializer<(string queryString, IDictionary<string, object> parameters)> serializer, ILogger logger = null)
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
            var serialized = _serializer.Serialize(query);

            _logger?.LogTrace("Executing Gremlin query {0}.", serialized.queryString);

            var serializer = Serializers.GetValue(
                query.Model,
                model => new GraphsonDeserializer(model));

            return _gremlinClient
                .SubmitAsync<JToken>(serialized.queryString, new Dictionary<string, object>(serialized.parameters))
                .ToAsyncEnumerable()
                .SelectMany(x => x
                    .ToAsyncEnumerable())
                .GraphsonDeserialize<TElement[]>(serializer)
                .SelectMany(x => x.ToAsyncEnumerable());
        }
    }
}
