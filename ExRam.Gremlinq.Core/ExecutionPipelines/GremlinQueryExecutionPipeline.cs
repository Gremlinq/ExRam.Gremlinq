using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionPipeline
    {
        private sealed class GremlinQueryExecutionPipelineImpl : IGremlinQueryExecutionPipeline
        {
            public GremlinQueryExecutionPipelineImpl(
                IGremlinQuerySerializer serializer,
                IGremlinQueryExecutor executor,
                IGremlinQueryExecutionResultDeserializer deserializer)
            {
                Executor = executor;
                Serializer = serializer;
                Deserializer = deserializer;
            }

            public IGremlinQueryExecutionPipeline ConfigureDeserializer(Func<IGremlinQueryExecutionResultDeserializer, IGremlinQueryExecutionResultDeserializer> configurator) => new GremlinQueryExecutionPipelineImpl(Serializer, Executor, configurator(Deserializer));

            public IGremlinQueryExecutionPipeline ConfigureSerializer(Func<IGremlinQuerySerializer, IGremlinQuerySerializer> configurator) => new GremlinQueryExecutionPipelineImpl(configurator(Serializer), Executor, Deserializer);

            public IGremlinQueryExecutionPipeline ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> configurator) => new GremlinQueryExecutionPipelineImpl(Serializer, configurator(Executor), Deserializer);

            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQueryBase<TElement> query)
            {
                var serialized = Serializer
                    .Serialize(query);

                if (serialized == null)
                    return AsyncEnumerableEx.Throw<TElement>(new Exception("Can't serialize query."));

                return Executor
                    .Execute(serialized)
                    .SelectMany(executionResult => Deserializer
                    .Deserialize<TElement>(executionResult, query.AsAdmin().Environment));
            }

            public IGremlinQuerySerializer Serializer { get; }
            public IGremlinQueryExecutor Executor { get; }
            public IGremlinQueryExecutionResultDeserializer Deserializer { get; }
        }

        public static IGremlinQueryExecutionPipeline UseSerializer(this IGremlinQueryExecutionPipeline pipeline, IGremlinQuerySerializer serializer) => pipeline.ConfigureSerializer(_ => serializer);

        public static IGremlinQueryExecutionPipeline UseDeserializer(this IGremlinQueryExecutionPipeline pipeline, IGremlinQueryExecutionResultDeserializer deserializer) => pipeline.ConfigureDeserializer(_ => deserializer);

        public static IGremlinQueryExecutionPipeline UseExecutor(this IGremlinQueryExecutionPipeline pipeline, IGremlinQueryExecutor executor) => pipeline.ConfigureExecutor(_ => executor);

        public static readonly IGremlinQueryExecutionPipeline Invalid = new GremlinQueryExecutionPipelineImpl(
            GremlinQuerySerializer.Invalid,
            GremlinQueryExecutor.Invalid,
            GremlinQueryExecutionResultDeserializer.Invalid);

        public static readonly IGremlinQueryExecutionPipeline Empty = new GremlinQueryExecutionPipelineImpl(
            GremlinQuerySerializer.Identity,
            GremlinQueryExecutor.Empty,
            GremlinQueryExecutionResultDeserializer.Empty);
        
        public static readonly IGremlinQueryExecutionPipeline EchoGraphson = Invalid
            .UseSerializer(GremlinQuerySerializer.Default)
            .UseExecutor(GremlinQueryExecutor.Echo)
            .UseDeserializer(GremlinQueryExecutionResultDeserializer.ToGraphson);

        public static readonly IGremlinQueryExecutionPipeline EchoGroovy = EchoGraphson
            .ConfigureSerializer(serializer => serializer.ToGroovy())
            .UseDeserializer(GremlinQueryExecutionResultDeserializer.ToString);
    }
}
