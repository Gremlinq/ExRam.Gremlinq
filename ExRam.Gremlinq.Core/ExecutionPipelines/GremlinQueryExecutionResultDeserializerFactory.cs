using System;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionResultDeserializerFactory<TExecutionResult>
    {
        private sealed class InvalidGremlinQueryExecutionResultDeserializerFactory : IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult>
        {
            public IGremlinQueryExecutionResultDeserializer<TExecutionResult> Get(IGraphModel model)
            {
                throw new InvalidOperationException();
            }
        }

        public static readonly IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> Invalid = new InvalidGremlinQueryExecutionResultDeserializerFactory();
    }
}
