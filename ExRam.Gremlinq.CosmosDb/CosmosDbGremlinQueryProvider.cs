using System.Collections.Generic;
using System.Reactive;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;

namespace ExRam.Gremlinq.CosmosDb
{
    public sealed class CosmosDbGremlinQueryProvider : INativeGremlinQueryProvider<JToken>
    {
        // ReSharper disable once InconsistentNaming
        private sealed class NullGraphSSON2Reader : GraphSON2Reader
        {
            public override dynamic ToObject(JToken jToken)
            {
                return new[] { jToken };
            }
        }

        private readonly INativeGremlinQueryProvider<JToken> _baseProvider;

        public CosmosDbGremlinQueryProvider(IOptions<CosmosDbGraphConfiguration> configuration, ILogger logger)
        {
            this._baseProvider = 
                new GremlinClient(
                    new GremlinServer(
                        configuration.Value.EndPoint,
                        443,
                        true,
                        "/dbs/" + configuration.Value.Database + "/colls/" + configuration.Value.GraphName,
                        configuration.Value.AuthKey),
                    new NullGraphSSON2Reader(),
                    new GraphSON2Writer(),
                    GremlinClient.GraphSON2MimeType)
                .ToNativeGremlinQueryProvider(
                    configuration.Value.TraversalSource,
                    logger);
        }

        public IAsyncEnumerable<JToken> Execute(string query, IDictionary<string, object> parameters)
        {
            return this._baseProvider.Execute(query, parameters);
        }

        public IGremlinQuery<Unit> TraversalSource => this._baseProvider.TraversalSource;
    }
}
