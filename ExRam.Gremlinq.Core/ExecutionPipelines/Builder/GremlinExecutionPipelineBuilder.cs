using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Providers;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinExecutionPipelineBuilder
    {
        public static IGremlinExecutionPipelineBuilderStage2<GroovySerializedGremlinQuery> AddGroovySerialization(this IGremlinExecutionPipelineBuilder builder)
        {
            return builder.AddSerializer(GremlinQuerySerializer<GroovySerializedGremlinQuery>.FromVisitor<GroovyGremlinQueryElementVisitor>());
        }

        public static IGremlinQueryExecutionPipeline AddGraphsonDeserialization(this IGremlinExecutionPipelineBuilderStage3<JToken> builder, params JsonConverter[] additionalConverters)
        {
            return builder.AddDeserializerFactory(new GraphsonDeserializerFactory(additionalConverters));
        }

        public static readonly IGremlinExecutionPipelineBuilder Default = new GremlinQueryExecutionPipeline<Unit, Unit>(
            GremlinQuerySerializer<Unit>.Invalid,
            GremlinQueryExecutor<Unit, Unit>.Invalid,
            GremlinQueryExecutionResultDeserializerFactory<Unit>.Invalid);
    }
}
