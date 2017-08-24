using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Graphs;

namespace ExRam.Gremlinq.Azure
{
    public sealed class DocumentClientCosmosDbSession : ICosmosDbSession
    {
        private readonly DocumentClient _client;
        private readonly DocumentCollection _graph;

        public DocumentClientCosmosDbSession(DocumentClient client, DocumentCollection graph)
        {
            this._client = client;
            this._graph = graph;
        }

        public IDocumentQuery<T> CreateGremlinQuery<T>(string gremlinExpression, FeedOptions feedOptions = null, GraphSONMode graphSONMode = GraphSONMode.Compact)
        {
            return this._client.CreateGremlinQuery<T>(this._graph, gremlinExpression, feedOptions, graphSONMode);
        }
    }
}
