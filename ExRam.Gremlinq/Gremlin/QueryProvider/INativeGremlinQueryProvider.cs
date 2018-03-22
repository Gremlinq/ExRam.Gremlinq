using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface INativeGremlinQueryProvider<out TNative> : IHasTraversalSource
    {
        IAsyncEnumerable<TNative> Execute(string query, IDictionary<string, object> parameters);
    }
}