using System;
using System.Collections.Generic;
using System.Linq;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IGremlinClient _gremlinClient;
        private readonly IGraphsonSerializerFactory _serializerFactory;
        private readonly IGremlinQuerySerializer<SerializedGremlinQuery> _serializer;

        public WebSocketGremlinQueryExecutor(
            IGremlinClient client,
            IGremlinQuerySerializer<SerializedGremlinQuery> serializer,
            IGraphsonSerializerFactory serializerFactory,
            ILogger logger = null)
        {
            _logger = logger;
            _gremlinClient = client;
            _serializer = serializer;
            _serializerFactory = serializerFactory;
        }

        public void Dispose()
        {
            _gremlinClient.Dispose();
        }

        public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
        {
            var serialized = _serializer.Serialize(query);

            _logger?.LogTrace("Executing Gremlin query {0}.", serialized.QueryString);
            
            return _gremlinClient
                .SubmitAsync<JToken>(serialized.QueryString, new Dictionary<string, object>(serialized.Bindings))
                .ToAsyncEnumerable()
                .SelectMany(x => x
                    .ToAsyncEnumerable())
                .GraphsonDeserialize<TElement[]>(_serializerFactory.Get(query.Model))
                .SelectMany(x => x.ToAsyncEnumerable());
        }
    }
}
