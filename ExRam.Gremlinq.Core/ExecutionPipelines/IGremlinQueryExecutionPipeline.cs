using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipeline
    {
        IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query);
    }

    public interface IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> : IGremlinQueryExecutionPipeline
    {
        IGremlinQuerySerializer<TSerializedQuery> Serializer { get; }
        IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>  Executor { get; }
        IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> DeserializerFactory { get; }
    }
}
