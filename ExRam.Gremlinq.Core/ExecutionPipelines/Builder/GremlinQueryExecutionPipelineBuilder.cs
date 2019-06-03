using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Providers;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionPipelineBuilder
    {
        public static IGremlinQueryExecutionPipelineBuilderWithSerializer<GroovySerializedGremlinQuery> AddGroovySerialization(this IGremlinQueryExecutionPipelineBuilder builder)
        {
            return builder.AddSerializer(GremlinQuerySerializer<GroovySerializedGremlinQuery>.FromVisitor<GroovyGremlinQueryElementVisitor>());
        }

        public static IGremlinQueryExecutionPipeline AddGraphsonDeserialization(this IGremlinQueryExecutionPipelineBuilderWithExecutor<JToken> builder, params JsonConverter[] additionalConverters)
        {
            return builder.AddDeserializerFactory(new GraphsonDeserializerFactory(additionalConverters));
        }

        public static readonly IGremlinQueryExecutionPipelineBuilder Default = new GremlinQueryExecutionPipeline<Unit, Unit>(
            GremlinQuerySerializer<Unit>.Invalid,
            GremlinQueryExecutor<Unit, Unit>.Invalid,
            GremlinQueryExecutionResultDeserializerFactory<Unit>.Invalid);
    }
}
