using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionResultDeserializer
    {
        IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, IGremlinQueryEnvironment environment);

        IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation);
    }
}
