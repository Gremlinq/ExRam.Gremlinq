using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryExecutionPipeline
    {
        IGremlinQueryExecutionPipeline ConfigureSerializer(Func<IGremlinQuerySerializer, IGremlinQuerySerializer> serializerTransformation);
        IGremlinQueryExecutionPipeline ConfigureDeserializer(Func<IGremlinQueryExecutionResultDeserializer, IGremlinQueryExecutionResultDeserializer> deserializerTransformation);
        IGremlinQueryExecutionPipeline ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation);

        IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQueryBase<TElement> query);

        IGremlinQueryExecutor Executor { get; }
        IGremlinQuerySerializer Serializer { get; }
        IGremlinQueryExecutionResultDeserializer Deserializer { get; }
    }
}
