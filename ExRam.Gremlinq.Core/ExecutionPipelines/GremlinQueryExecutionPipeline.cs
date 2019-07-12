using System;
using System.Collections.Generic;
using System.Linq;
using ExRam.Gremlinq.Core.Serialization;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionPipeline
    {
        public static readonly IGremlinQueryExecutionPipeline Invalid = new GremlinQueryExecutionPipeline<GroovySerializedGremlinQuery, Unit>(
            GremlinQuerySerializer<GroovySerializedGremlinQuery>.FromVisitor<GroovyGremlinQueryElementVisitor>(),
            GremlinQueryExecutor<GroovySerializedGremlinQuery, Unit>.Invalid,
            GremlinQueryExecutionResultDeserializer<Unit>.Invalid);
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
            IGremlinQueryExecutionResultDeserializer<TExecutionResult> deserializer)
        {
            Executor = executor;
            Serializer = serializer;
            Deserializer = deserializer;
        }

        public IGremlinQueryExecutionPipelineBuilderWithSerializer<TNewSerializedQuery> UseSerializer<TNewSerializedQuery>(IGremlinQuerySerializer<TNewSerializedQuery> serializer)
        {
            return new GremlinQueryExecutionPipeline<TNewSerializedQuery, TExecutionResult>(serializer, GremlinQueryExecutor<TNewSerializedQuery, TExecutionResult>.Invalid, Deserializer);
        }

        public IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> ConfigureDeserializer(Func<IGremlinQueryExecutionResultDeserializer<TExecutionResult>, IGremlinQueryExecutionResultDeserializer<TExecutionResult>> configurator)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult>(Serializer, Executor, configurator(Deserializer));
        }

        public IGremlinQueryExecutionPipelineBuilderWithSerializer<TSerializedQuery> ConfigureSerializer(Func<IGremlinQuerySerializer<TSerializedQuery>, IGremlinQuerySerializer<TSerializedQuery>> configurator)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult>(configurator(Serializer), Executor, Deserializer);
        }

        public IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TNewExecutionResult> UseExecutor<TNewExecutionResult>(IGremlinQueryExecutor<TSerializedQuery, TNewExecutionResult> executor)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TNewExecutionResult>(Serializer, executor, GremlinQueryExecutionResultDeserializer<TNewExecutionResult>.Invalid);
        }

        public IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, TExecutionResult> ConfigureExecutor(Func<IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>, IGremlinQueryExecutor<TSerializedQuery, TExecutionResult>> configurator)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult>(Serializer, configurator(Executor), Deserializer);
        }

        public IGremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult> UseDeserializer(IGremlinQueryExecutionResultDeserializer<TExecutionResult> deserializerFactory)
        {
            return new GremlinQueryExecutionPipeline<TSerializedQuery, TExecutionResult>(Serializer, Executor, deserializerFactory);
        }

        public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
        {
            return Executor
                .Execute(Serializer
                    .Serialize(query))
                .SelectMany(executionResult => Deserializer
                    .Deserialize<TElement>(executionResult, query.AsAdmin().Environment));
        }

        public IGremlinQuerySerializer<TSerializedQuery> Serializer { get; }
        public IGremlinQueryExecutor<TSerializedQuery, TExecutionResult> Executor { get; }
        public IGremlinQueryExecutionResultDeserializer<TExecutionResult> Deserializer { get; }
    }
}
