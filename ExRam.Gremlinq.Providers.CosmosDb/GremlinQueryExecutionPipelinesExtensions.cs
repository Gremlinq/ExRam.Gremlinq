using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Providers;
using ExRam.Gremlinq.Providers.WebSocket;
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
                .UseGraphsonDeserializer(new TimespanConverter());
        }

        public static IGremlinQueryExecutionPipeline UseCosmosDbSerializer(this IGremlinQueryExecutionPipeline pipeline)
        {
            return pipeline
                .UseSerializer(GremlinQuerySerializerBuilder.Invalid
                    .AddGremlinSteps()
                    .AddGroovy()
                    .AddCosmosDbWorkarounds()
                    .Build());
        }

        public static IGremlinQueryExecutionPipeline UseCosmosDbExecutor(this IGremlinQueryExecutionPipeline builder, string hostname, string database, string graphName, string authKey, ILogger logger, int port = 443)
        {
            return builder.UseCosmosDbExecutor(hostname, port, true, database, graphName, authKey, logger);
        }

        public static IGremlinQueryExecutionPipeline UseCosmosDbEmulatorExecutor(this IGremlinQueryExecutionPipeline builder, string hostname, string database, string graphName, string authKey, ILogger logger, int port = 8901)
        {
            return builder.UseCosmosDbExecutor(hostname, port, false, database, graphName, authKey, logger);
        }

        private static IGremlinQueryExecutionPipeline UseCosmosDbExecutor(this IGremlinQueryExecutionPipeline builder, string hostname, int port, bool enableSsl, string database, string graphName, string authKey, ILogger logger)
        {
            return builder
                .UseWebSocketExecutor(
                    hostname,
                    port,
                    enableSsl,
                    $"/dbs/{database}/colls/{graphName}",
                    authKey,
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
