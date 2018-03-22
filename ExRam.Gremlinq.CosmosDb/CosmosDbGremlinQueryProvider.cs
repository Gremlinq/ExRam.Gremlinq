using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;

namespace ExRam.Gremlinq.CosmosDb
{
    public sealed class CosmosDbGremlinQueryProvider : INativeGremlinQueryProvider<string>
    {
        private readonly ILogger _logger;
        private readonly DocumentClient _client;
        private readonly Task<ResourceResponse<DocumentCollection>> _graph;

        public CosmosDbGremlinQueryProvider(IOptions<CosmosDbGraphConfiguration> configuration, ILogger logger)
        {
            this._logger = logger;
            this._client =  new DocumentClient(
                new Uri(configuration.Value.EndPoint),
                configuration.Value.AuthKey,
                new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });

            this._graph = this._client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(configuration.Value.Database),
                new DocumentCollection { Id = configuration.Value.GraphName },
                new RequestOptions { OfferThroughput = 1000 });

            this.TraversalSource = GremlinQuery.Create(configuration.Value.TraversalSource);
        }

        public IAsyncEnumerable<string> Execute(string query, IDictionary<string, object> parameters)
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

            return this._graph
                .Map(graph => this._client.CreateGremlinQuery<JToken>(graph, query))
                .ToAsyncEnumerable()
                .Repeat()
                .TakeWhile(documentQuery => documentQuery.HasMoreResults)
                // ReSharper disable once ImplicitlyCapturedClosure
                .SelectMany(async (documentQuery, ct) =>
                {
                    try
                    {
                        return await documentQuery.ExecuteNextAsync<JToken>(ct).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(query, ex);
                    }
                })
                .SelectMany(x => x.ToAsyncEnumerable())
                .Select(x => x.ToString());
        }

        public IGremlinQuery<Unit> TraversalSource { get; }
    }
}
