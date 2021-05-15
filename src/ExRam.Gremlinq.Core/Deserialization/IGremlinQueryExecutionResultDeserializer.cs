using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IGremlinQueryExecutionResultDeserializer
    {
        IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, IGremlinQueryEnvironment environment);

        IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IGremlinQueryFragmentDeserializer, IGremlinQueryFragmentDeserializer> transformation);
    }
}
