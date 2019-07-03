using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Providers;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionPipelineBuilder
    {
        public static IGremlinQueryExecutionPipelineBuilderWithSerializer<GroovySerializedGremlinQuery> UseGroovySerialization(this IGremlinQueryExecutionPipelineBuilder builder)
        {
            return builder.UseSerializer(GremlinQuerySerializer<GroovySerializedGremlinQuery>.FromVisitor<GroovyGremlinQueryElementVisitor>());
        }

        public static IGremlinQueryExecutionPipeline<TSerializedQuery, JToken> UseGraphsonDeserialization<TSerializedQuery>(this IGremlinQueryExecutionPipelineBuilderWithExecutor<TSerializedQuery, JToken> builder, params JsonConverter[] additionalConverters)
        {
            return builder.UseDeserializerFactory(new GraphsonDeserializerFactory(additionalConverters));
        }

        public static IGremlinQueryExecutionPipeline<GroovySerializedGremlinQuery, GroovySerializedGremlinQuery> EchoGremlinQueryAsString(this IGremlinQueryExecutionPipelineBuilder builder)
        {
            return builder
                .UseSerializer(GremlinQuerySerializer<GroovySerializedGremlinQuery>
                    .FromVisitor<GroovyGremlinQueryElementVisitor>())
                .UseExecutor(GremlinQueryExecutor
                    .Echo<GroovySerializedGremlinQuery>())
                .UseDeserializerFactory(GremlinQueryExecutionResultDeserializerFactory
                    .ToStringDeserializerFactory<GroovySerializedGremlinQuery>());
        }

        public static readonly IGremlinQueryExecutionPipelineBuilder Default = new GremlinQueryExecutionPipeline<Unit, Unit>(
            GremlinQuerySerializer<Unit>.Invalid,
            GremlinQueryExecutor<Unit, Unit>.Invalid,
            GremlinQueryExecutionResultDeserializerFactory<Unit>.Invalid);
    }
}
