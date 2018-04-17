using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;

namespace ExRam.Gremlinq.CosmosDb
{
    public sealed class CosmosDbGremlinQueryProvider : INativeGremlinQueryProvider<JToken>
    {
        private readonly ILogger _logger;
        private readonly INativeGremlinQueryProvider<JToken> _baseProvider;

        public CosmosDbGremlinQueryProvider(IOptions<CosmosDbGraphConfiguration> configuration, ILogger logger)
        {
            this._logger = logger;

            this._baseProvider = 
                new GremlinClient(
                    new GremlinServer(
                        configuration.Value.EndPoint,
                        443,
                        true,
                        "/dbs/" + configuration.Value.Database + "/colls/" + configuration.Value.GraphName,
                        configuration.Value.AuthKey))
                .ToNativeGremlinQueryProvider(
                    configuration.Value.TraversalSource,
                    logger);
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

            return this._baseProvider.Execute(query, new Dictionary<string, object>());
        }

        public IGremlinQuery<Unit> TraversalSource => this._baseProvider.TraversalSource;
    }
}
