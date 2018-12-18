using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using NullGuard;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public sealed class CosmosDbGraphsonSerializerFactory : IGraphsonSerializerFactory
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

        private readonly ConditionalWeakTable<IGraphModel, GraphsonDeserializer> _serializers = new ConditionalWeakTable<IGraphModel, GraphsonDeserializer>();

        public JsonSerializer Get(IGraphModel model)
        {
            return _serializers.GetValue(
                model,
                closureModel => new GraphsonDeserializer(closureModel, new TimespanConverter()));
        }
    }
}
