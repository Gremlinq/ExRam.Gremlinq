using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Providers;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySourceExtensions
    {
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

        private static readonly Step NoneWorkaround = new NotStep(GremlinQuery.Anonymous(GremlinQueryEnvironment.Default).Identity());

        public static IConfigurableGremlinQuerySource UseCosmosDb(this IConfigurableGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 443)
        {
            return source.CreateCosmosDbGremlinQuerySource(hostname, database, graphName, authKey, port, true);
        }

        public static IConfigurableGremlinQuerySource UseCosmosDbEmulator(this IConfigurableGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 8901)
        {
            return source.CreateCosmosDbGremlinQuerySource(hostname, database, graphName, authKey, port, false);
        }

        private static IConfigurableGremlinQuerySource CreateCosmosDbGremlinQuerySource(this IConfigurableGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port, bool enableSsl)
        {
            return source
               .ConfigureExecutionPipeline(builder => builder
                   .UseSerializer(GremlinQuerySerializerBuilder.Groovy
                        .OverrideAtom<SkipStep>((step, assembler, overridden, recurse) => recurse(new RangeStep(step.Count, -1)))
                        .OverrideAtom<NoneStep>((step, assembler, overridden, recurse) => recurse(NoneWorkaround))
                        .OverrideAtom<LimitStep>((step, assembler, overridden, recurse) =>
                        {
                            // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                            if (step.Count > int.MaxValue)
                                throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Limit' outside the range of a 32-bit-integer.");

                            overridden(step);
                        })
                        .OverrideAtom<TailStep>((step, assembler, overridden, recurse) =>
                        {
                            // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                            if (step.Count > int.MaxValue)
                                throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Tail' outside the range of a 32-bit-integer.");

                            overridden(step);
                        })
                        .OverrideAtom<RangeStep>((step, assembler, overridden, recurse) =>
                        {
                            // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                            if (step.Lower > int.MaxValue || step.Upper > int.MaxValue)
                                throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Range' outside the range of a 32-bit-integer.");

                            overridden(step);
                        })
                        .OverrideAtom<long>((l, assembler, overridden, recurse) =>
                        {
                            // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                            recurse((int)l);
                        })
                        .Build())
                   .AddWebSocketExecutor(
                       hostname,
                       port,
                       enableSsl,
                       $"/dbs/{database}/colls/{graphName}",
                       authKey,
                       GraphsonVersion.V2, new Dictionary<Type, IGraphSONSerializer>
                       {
                            { typeof(TimeSpan), new TimeSpanSerializer() }
                       },
                       default,
                       source.Logger)
                   .UseGraphsonDeserialization(new TimespanConverter()));
        }
    }
}
