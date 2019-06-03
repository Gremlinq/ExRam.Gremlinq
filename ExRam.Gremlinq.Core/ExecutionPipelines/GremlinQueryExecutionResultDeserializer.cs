using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionResultDeserializer<TExecutionResult>
    {
        private sealed class InvalidQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer<TExecutionResult>
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(TExecutionResult result)
            {
                return AsyncEnumerableEx.Throw<TElement>(new InvalidOperationException($"{nameof(Deserialize)} must not be called on {nameof(GremlinQueryExecutionResultDeserializer<TExecutionResult>)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQueryExecutionResultDeserializer<TExecutionResult>)} on your {nameof(GremlinQuerySource)}."));
            }
        }

        public static readonly IGremlinQueryExecutionResultDeserializer<TExecutionResult> Invalid = new InvalidQueryExecutionResultDeserializer();
    }
}
