using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Graphs;

namespace Microsoft.Azure.Documents.Client
{
    public interface ICosmosDbSession
    {
        IDocumentQuery<T> CreateGremlinQuery<T>(string gremlinExpression, FeedOptions feedOptions = null, GraphSONMode graphSONMode = GraphSONMode.Compact);
    }
}