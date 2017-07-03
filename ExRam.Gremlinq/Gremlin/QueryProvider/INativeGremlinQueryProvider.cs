using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface INativeGremlinQueryProvider : IHasTraversalSource
    {
        IAsyncEnumerable<string> Execute(string query, IDictionary<string, object> parameters);
    }
}