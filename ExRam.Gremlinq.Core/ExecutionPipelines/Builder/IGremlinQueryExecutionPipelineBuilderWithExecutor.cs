using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TExecutionResult>
    {
        IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TExecutionResult> ConfigureExecutor(Func<IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>, IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>> configurator);

        IGremlinQueryExecutionPipeline UseDeserializerFactory(IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> deserializerFactory);
    }
}
