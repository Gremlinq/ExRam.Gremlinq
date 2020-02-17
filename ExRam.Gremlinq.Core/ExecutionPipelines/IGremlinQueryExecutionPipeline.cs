using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipeline
    {
        IGremlinQueryExecutionPipeline ConfigureSerializer(Func<IGremlinQuerySerializer, IGremlinQuerySerializer> serializerTransformation);
        IGremlinQueryExecutionPipeline ConfigureDeserializer(Func<IGremlinQueryExecutionResultDeserializer, IGremlinQueryExecutionResultDeserializer> deserializerTransformation);
        IGremlinQueryExecutionPipeline ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation);

        IGremlinQueryExecutor Executor { get; }
        IGremlinQuerySerializer Serializer { get; }
        IGremlinQueryExecutionResultDeserializer Deserializer { get; }
    }
}
