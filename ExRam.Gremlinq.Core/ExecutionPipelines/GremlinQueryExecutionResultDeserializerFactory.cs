using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionResultDeserializerFactory
    {
        private sealed class ToStringGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> : IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult>
        {
            private sealed class ToStringGremlinQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer<TExecutionResult>
            {
                public IAsyncEnumerable<TElement> Deserialize<TElement>(TExecutionResult result)
                {
                    if (!typeof(TElement).IsAssignableFrom(typeof(string)))
                        throw new NotSupportedException();

                    return AsyncEnumerableEx.Return((TElement)(object)result?.ToString());
                }
            }

            public IGremlinQueryExecutionResultDeserializer<TExecutionResult> Get(IGremlinQueryEnvironment environment)
            {
                return new ToStringGremlinQueryExecutionResultDeserializer();
            }
        }

        public static IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> ToStringDeserializerFactory<TExecutionResult>()
        {
            return new ToStringGremlinQueryExecutionResultDeserializerFactory<TExecutionResult>();
        }
    }

    public static class GremlinQueryExecutionResultDeserializerFactory<TExecutionResult>
    {
        private sealed class InvalidGremlinQueryExecutionResultDeserializerFactory : IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult>
        {
            public IGremlinQueryExecutionResultDeserializer<TExecutionResult> Get(IGremlinQueryEnvironment environment)
            {
                throw new InvalidOperationException();
            }
        }

        public static readonly IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> Invalid = new InvalidGremlinQueryExecutionResultDeserializerFactory();
    }
}
