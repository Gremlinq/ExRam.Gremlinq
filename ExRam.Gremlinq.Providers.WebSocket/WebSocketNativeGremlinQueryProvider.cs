using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public sealed class WebSocketNativeGremlinQueryProvider : INativeGremlinQueryProvider<JToken>
    {
        private sealed class NullGraphSonReader : GraphSON2Reader
        {
            public override dynamic ToObject(JToken jToken)
            {
                return new[] { jToken };
            }
        }

        private readonly ILogger _logger;
        private readonly GremlinClient _gremlinClient;

        public WebSocketNativeGremlinQueryProvider(string host, int port, string username, string password, string traversalSourceName, ILogger logger)
        {
            this._logger = logger;

            this._gremlinClient = new GremlinClient(new GremlinServer(host, port, true, username, password), new NullGraphSonReader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
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

        public IGremlinQuery<Unit> TraversalSource { get; }
    }
}
