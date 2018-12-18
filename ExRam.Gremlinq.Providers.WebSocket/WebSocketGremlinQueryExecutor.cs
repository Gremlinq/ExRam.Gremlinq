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
    public class WebSocketGremlinQueryExecutor : WebSocketGremlinQueryExecutor<GroovyGremlinQueryElementVisitor>
    {
        public WebSocketGremlinQueryExecutor(IGremlinClient client, IGraphsonSerializerFactory serializerFactory, ILogger logger = null) : base(client, serializerFactory, logger)
        {
        }
    }

    public class WebSocketGremlinQueryExecutor<TVisitor> : IGremlinQueryExecutor, IDisposable
        where TVisitor : IGremlinQueryElementVisitor<SerializedGremlinQuery>, new()
    {
        private readonly ILogger _logger;
        private readonly IGremlinClient _gremlinClient;
        private readonly IGraphsonSerializerFactory _serializerFactory;

        public WebSocketGremlinQueryExecutor(
            IGremlinClient client,
            IGraphsonSerializerFactory serializerFactory,
            ILogger logger = null)
        {
            _logger = logger;
            _gremlinClient = client;
            _serializerFactory = serializerFactory;
        }

        public void Dispose()
        {
            _gremlinClient.Dispose();
        }

        public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
        {
            var visitor = new TVisitor();

            visitor
                .Visit(query);

            var serialized = visitor.Build();

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
