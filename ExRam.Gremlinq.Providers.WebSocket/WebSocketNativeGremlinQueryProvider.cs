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
            foreach (var kvp in parameters.OrderByDescending(x => x.Key.Length))
            {
                var value = kvp.Value;

                switch (value)
                {
                    case Enum _:
                        value = (int)value;
                        break;
                    case DateTimeOffset x:
                        value = x.ToUniversalTime().ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffZ");
                        break;
                    case DateTime x:
                        value = x.ToUniversalTime().ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffZ");
                        break;
                    case TimeSpan x:
                        value = x.TotalSeconds;
                        break;
                    case byte[] x:
                        value = Convert.ToBase64String(x);
                        break;
                }

                value = value is string
                    ? $"'{value}'"
                    : value.ToString().ToLower();

                query = query.Replace(kvp.Key, (string)value);
            }

            this._logger.LogTrace("Executing Gremlin query {0}.", query);

            return this._gremlinClient
                .SubmitAsync<JToken>(query)
                .ToAsyncEnumerable()
                .SelectMany(x => x.ToAsyncEnumerable());
        }

        public IGremlinQuery<Unit> TraversalSource { get; }
    }
}
