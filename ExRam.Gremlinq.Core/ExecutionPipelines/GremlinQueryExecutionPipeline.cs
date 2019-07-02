using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionPipeline
    {
        public static readonly IGremlinQueryExecutionPipeline Invalid = new GremlinQueryExecutionPipeline<Unit, Unit>(
            GremlinQuerySerializer<Unit>.Invalid,
            GremlinQueryExecutor<Unit, Unit>.Invalid,
            GremlinQueryExecutionResultDeserializerFactory<Unit>.Invalid);
    }

    internal sealed class GremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> :
        IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult>,
        IGremlinQueryExecutionPipelineBuilder,
        IGremlinQueryExecutionPipelineBuilderWithSerializer<TSerializedQuery>,
        IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TExecutionResult>
    {
        public GremlinQueryExecutionPipeline(
            IGremlinQuerySerializer<TSerializedQuery> serializer,
            IGremlinQueryExecutor<TSerializedQuery, TExecutionResult> executor,
            IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> deserializerFactory)
        {
            Executor = executor;
            Serializer = serializer;
            DeserializerFactory = deserializerFactory;
        }

        public IGremlinQueryExecutionPipelineBuilderWithSerializer<TNewSerializedQuery> UseSerializer<TNewSerializedQuery>(IGremlinQuerySerializer<TNewSerializedQuery> serializer)
        {
            return new GremlinQueryExecutionPipeline<TNewSerializedQuery, TExecutionResult>(serializer, GremlinQueryExecutor<TNewSerializedQuery, TExecutionResult>.Invalid, DeserializerFactory);
        }

        public IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> ConfigureDeserializerFactory(Func<IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult>, IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult>> configurator)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult>(Serializer, Executor, configurator(DeserializerFactory));
        }

        public IGremlinQueryExecutionPipelineBuilderWithSerializer<TSerializedQuery> ConfigureSerializer(Func<IGremlinQuerySerializer<TSerializedQuery>, IGremlinQuerySerializer<TSerializedQuery>> configurator)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult>(configurator(Serializer), Executor, DeserializerFactory);
        }

        public IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TNewExecutionResult> UseExecutor<TNewExecutionResult>(IGremlinQueryExecutor<TSerializedQuery, TNewExecutionResult> executor)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TNewExecutionResult>(Serializer, executor, GremlinQueryExecutionResultDeserializerFactory<TNewExecutionResult>.Invalid);
        }

        public IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TExecutionResult> ConfigureExecutor(Func<IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>, IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>> configurator)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult>(Serializer, configurator(Executor), DeserializerFactory);
        }

        public IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> UseDeserializerFactory(IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> deserializerFactory)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult>(Serializer, Executor, deserializerFactory);
        }

        public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
        {
            var deserializer = DeserializerFactory.Get(query.AsAdmin().Model);

            return Executor
                .Execute(Serializer
                    .Serialize(query))
                .SelectMany(executionResult => deserializer
                    .Deserialize<TElement>(executionResult));
        }

        public IGremlinQuerySerializer<TSerializedQuery> Serializer { get; }
        public IGremlinQueryExecutor<TSerializedQuery, TExecutionResult> Executor { get; }
        public IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> DeserializerFactory { get; }
    }
}
