using System.Collections.Generic;
using System.Linq;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.CosmosDb
{
    public class CosmosDbNativeGremlinQueryProvider : INativeGremlinQueryProvider<JToken>
    {
        //https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
        private sealed class WorkaroundCosmosDbBugsQueryProvider : INativeGremlinQueryProvider<JToken>
        {
            private readonly INativeGremlinQueryProvider<JToken> _nativeGremlinQueryProviderImplementation;

            public WorkaroundCosmosDbBugsQueryProvider(INativeGremlinQueryProvider<JToken> nativeGremlinQueryProviderImplementation)
            {
                this._nativeGremlinQueryProviderImplementation = nativeGremlinQueryProviderImplementation;
            }

            public IAsyncEnumerable<JToken> Execute(string query, IDictionary<string, object> parameters)
            {
                parameters = parameters
                    .Select(kvp => kvp.Value is long l ? new KeyValuePair<string, object>(kvp.Key, (int)l) : kvp)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                return this._nativeGremlinQueryProviderImplementation.Execute(query, parameters);
            }
        }

        private readonly INativeGremlinQueryProvider<JToken> _baseProvider;

        public CosmosDbNativeGremlinQueryProvider(IGremlinClient client, ILogger logger)
        {
            this._baseProvider = new WorkaroundCosmosDbBugsQueryProvider(new GremlinClientNativeGremlinQueryProvider(client, logger));
        }

        public IAsyncEnumerable<JToken> Execute(string query, IDictionary<string, object> parameters)
        {
            return this._baseProvider.Execute(query, parameters);
        }
    }
}