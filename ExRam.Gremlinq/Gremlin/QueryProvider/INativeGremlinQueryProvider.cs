using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface INativeGremlinQueryProvider
    {
        IAsyncEnumerable<string> Execute(string query, IDictionary<string, object> parameters);
        
        IGremlinQuery TraversalSource { get; }
    }
}