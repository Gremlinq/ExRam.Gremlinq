using System;
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

        IGremlinQueryExecutionPipelineBuilderWithSerializer<TSerializedQuery> ConfigureSerializer(Func<IGremlinQuerySerializer<TSerializedQuery>, IGremlinQuerySerializer<TSerializedQuery>> configurator);
        IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TExecutionResult> ConfigureExecutor(Func<IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>, IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>> configurator);
        IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> ConfigureDeserializerFactory(Func<IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult>, IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult>> configurator);
    }
}
