using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface INativeGremlinQueryProvider<out TNative>
    {
        IAsyncEnumerable<TNative> Execute(string query, IDictionary<string, object> parameters);
    }
}