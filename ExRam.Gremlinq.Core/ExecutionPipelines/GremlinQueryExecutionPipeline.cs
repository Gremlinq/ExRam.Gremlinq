using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionPipeline
    {
        private sealed class InvalidGremlinQueryExecutionPipeline : IGremlinQueryExecutionPipeline
        {
            public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
            {
                return AsyncEnumerableEx.Throw<TElement>(new InvalidOperationException($"'{nameof(Execute)}' must not be called on GremlinQueryExecutionPipeline.Invalid. If you are getting this exception while executing a query, set a proper GremlinQueryExecutionPipeline."));    //TODO: Refine message
            }
        }

        public static readonly IGremlinQueryExecutionPipeline Invalid = new GremlinQueryExecutionPipeline<Unit, Unit>(
            GremlinQuerySerializer<Unit>.Invalid,
            GremlinQueryExecutor<Unit, Unit>.Invalid,
            GremlinQueryExecutionResultDeserializerFactory<Unit>.Invalid);
    }

    internal sealed class GremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> :
        IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult>,
        IGremlinExecutionPipelineBuilderStage1,
        IGremlinExecutionPipelineBuilderStage2<TSerializedQuery>,
        IGremlinExecutionPipelineBuilderStage3<TExecutionResult>
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

        public IGremlinExecutionPipelineBuilderStage2<TNewSerializedQuery> AddSerializer<TNewSerializedQuery>(IGremlinQuerySerializer<TNewSerializedQuery> serializer)
        {
            return new GremlinQueryExecutionPipeline<TNewSerializedQuery, TExecutionResult>(serializer, GremlinQueryExecutor<TNewSerializedQuery, TExecutionResult>.Invalid, DeserializerFactory);
        }

        public IGremlinExecutionPipelineBuilderStage3<TNewExecutionResult> AddExecutor<TNewExecutionResult>(IGremlinQueryExecutor<TSerializedQuery, TNewExecutionResult> executor)
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
