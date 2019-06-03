using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionResultDeserializer<in TExecutionResult>
    {
        IAsyncEnumerable<TElement> Deserialize<TElement>(TExecutionResult result);
    }
}
