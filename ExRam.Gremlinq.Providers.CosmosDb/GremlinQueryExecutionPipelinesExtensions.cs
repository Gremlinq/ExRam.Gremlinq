using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Providers;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionPipelinesExtensions
    {
        private sealed class TimespanConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(TimeSpan);
            }

            public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
            {
                return TimeSpan.FromMilliseconds(serializer.Deserialize<long>(reader));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanRead => true;
            public override bool CanWrite => true;
        }

        private sealed class TimeSpanSerializer : IGraphSONSerializer, IGraphSONDeserializer
        {
            public Dictionary<string, dynamic> Dictify(dynamic objectData, GraphSONWriter writer)
            {
                TimeSpan value = objectData;
                return GraphSONUtil.ToTypedValue("Double", value.TotalMilliseconds);
            }

            public dynamic Objectify(JToken graphsonObject, GraphSONReader reader)
            {
                var duration = graphsonObject.ToObject<double>();
                return TimeSpan.FromMilliseconds(duration);
            }
        }

        public static IGremlinQueryExecutionPipeline UseCosmosDbDeserializer(this IGremlinQueryExecutionPipeline pipeline)
        {
            return pipeline
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.GraphsonWithJsonConverters(new TimespanConverter()));
        }

        public static IGremlinQueryExecutionPipeline UseCosmosDbSerializer(this IGremlinQueryExecutionPipeline pipeline)
        {
            return pipeline
                .ConfigureSerializer(serializer => serializer
                    .UseCosmosDbWorkarounds()
                    .ToGroovy());
        }

        public static IGremlinQueryExecutionPipeline UseCosmosDbExecutor(this IGremlinQueryExecutionPipeline pipeline, string hostname, string database, string graphName, string authKey, ILogger logger, int port = 443)
        {
            return pipeline.UseCosmosDbExecutor(hostname, port, true, database, graphName, authKey, logger);
        }

        public static IGremlinQueryExecutionPipeline UseCosmosDbEmulatorExecutor(this IGremlinQueryExecutionPipeline pipeline, string hostname, string database, string graphName, string authKey, ILogger logger, int port = 8901)
        {
            return pipeline.UseCosmosDbExecutor(hostname, port, false, database, graphName, authKey, logger);
        }

        private static IGremlinQueryExecutionPipeline UseCosmosDbExecutor(this IGremlinQueryExecutionPipeline pipeline, string hostname, int port, bool enableSsl, string database, string graphName, string authKey, ILogger logger)
        {
            return pipeline
                .UseWebSocketExecutor(
                    hostname,
                    port,
                    enableSsl,
                    $"/dbs/{database}/colls/{graphName}",
                    authKey,
                    "g",
                    GraphsonVersion.V2,
                    new Dictionary<Type, IGraphSONSerializer>
                    {
                        {typeof(TimeSpan), new TimeSpanSerializer()}
                    },
                    default,
                    logger);
        }
    }
}
