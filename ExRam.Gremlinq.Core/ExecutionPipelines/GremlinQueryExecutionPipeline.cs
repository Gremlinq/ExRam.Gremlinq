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
        IGremlinQueryExecutionPipelineBuilderStage2<TSerializedQuery>,
        IGremlinQueryExecutionPipelineBuilderStage3<TExecutionResult>
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

        public IGremlinQueryExecutionPipelineBuilderStage2<TNewSerializedQuery> AddSerializer<TNewSerializedQuery>(IGremlinQuerySerializer<TNewSerializedQuery> serializer)
        {
            return new GremlinQueryExecutionPipeline<TNewSerializedQuery, TExecutionResult>(serializer, GremlinQueryExecutor<TNewSerializedQuery, TExecutionResult>.Invalid, DeserializerFactory);
        }

        public IGremlinQueryExecutionPipelineBuilderStage3<TNewExecutionResult> AddExecutor<TNewExecutionResult>(IGremlinQueryExecutor<TSerializedQuery, TNewExecutionResult> executor)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TNewExecutionResult>(Serializer, executor, GremlinQueryExecutionResultDeserializerFactory<TNewExecutionResult>.Invalid);
        }

        public IGremlinQueryExecutionPipeline AddDeserializerFactory(IGremlinQueryExecutionResultDeserializerFactory<TExecutionResult> deserializerFactory)
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
