using System;
using System.Collections.Generic;
using ExRam.Gremlinq.Core.Serialization;
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

        private sealed class CosmosDbGroovyGremlinQueryElementVisitor : GroovyGremlinQueryElementVisitor
        {
            private static readonly Step NoneWorkaround = new NotStep(GremlinQuery.Anonymous(GremlinQueryEnvironment.Default).Identity());

            public override void Visit(SkipStep step)
            {
                // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                if (step.Count > int.MaxValue)
                    throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Skip' outside the range of a 32-bit-integer.");

                base.Visit(step);
            }

            public override void Visit(LimitStep step)
            {
                // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                if (step.Count > int.MaxValue)
                    throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Limit' outside the range of a 32-bit-integer.");

                base.Visit(step);
            }

            public override void Visit(TailStep step)
            {
                // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                if (step.Count > int.MaxValue)
                    throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Tail' outside the range of a 32-bit-integer.");

                base.Visit(step);
            }

            public override void Visit(RangeStep step)
            {
                // Workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
                if (step.Lower > int.MaxValue || step.Upper > int.MaxValue)
                    throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Range' outside the range of a 32-bit-integer.");

                base.Visit(step);
            }

            protected override void Method(string methodName, object parameter)
            {
                base.Method(
                    methodName,
                    parameter is long l ? (int)l : parameter);
            }

            protected override void Method(string methodName, object parameter1, object parameter2)
            {
                base.Method(
                    methodName,
                    parameter1 is long l1 ? (int)l1 : parameter1,
                    parameter2 is long l2 ? (int)l2 : parameter2);
            }

            protected override void Method(string methodName, object parameter1, object parameter2, object parameter3)
            {
                base.Method(
                    methodName,
                    parameter1 is long l1 ? (int)l1 : parameter1,
                    parameter2 is long l2 ? (int)l2 : parameter2,
                    parameter2 is long l3 ? (int)l3 : parameter3);
            }

            public override void Visit(NoneStep step)
            {
                Visit(NoneWorkaround);
            }
        }

        public static IConfigurableGremlinQuerySource UseCosmosDb(this IConfigurableGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 443)
        {
            return source
                .UseExecutionPipeline(builder => builder
                    .AddSerializer(GremlinQuerySerializer<GroovySerializedGremlinQuery>
                        .FromVisitor<CosmosDbGroovyGremlinQueryElementVisitor>())
                    .AddWebSocketExecutor(
                        hostname,
                        port,
                        true,
                        $"/dbs/{database}/colls/{graphName}",
                        authKey,
                        GraphsonVersion.V2, new Dictionary<Type, IGraphSONSerializer>
                        {
                            { typeof(TimeSpan), new TimeSpanSerializer() }
                        },
                        default,
                        source.Logger)
                    .AddGraphsonDeserialization(new TimespanConverter()));
        }
    }
}
