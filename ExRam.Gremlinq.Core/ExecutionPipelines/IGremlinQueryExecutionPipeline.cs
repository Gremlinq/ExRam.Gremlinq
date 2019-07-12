using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipeline
    {
        IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query);
    }

    public interface IGremlinQueryExecutionPipeline<out TSerializedQuery> : IGremlinQueryExecutionPipeline
    {
        IGremlinQuerySerializer<TSerializedQuery> Serializer { get; }
    }

    public interface IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> : IGremlinQueryExecutionPipeline<TSerializedQuery>
    {
        IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>  Executor { get; }
        IGremlinQueryExecutionResultDeserializer<TExecutionResult> Deserializer { get; }

        IGremlinQueryExecutionPipelineBuilderWithSerializer<TSerializedQuery> ConfigureSerializer(Func<IGremlinQuerySerializer<TSerializedQuery>, IGremlinQuerySerializer<TSerializedQuery>> configurator);
        IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TExecutionResult> ConfigureExecutor(Func<IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>, IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>> configurator);
        IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> ConfigureDeserializer(Func<IGremlinQueryExecutionResultDeserializer<TExecutionResult>, IGremlinQueryExecutionResultDeserializer<TExecutionResult>> configurator);
    }
}
