using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Serialization;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryEnvironmentExtensions
    {
        public static IGremlinQueryEnvironment StoreTimeSpansAsNumbers(this IGremlinQueryEnvironment environment)
        {
            return environment
                .ConfigureSerializer(static serializer => serializer
                    .ConfigureFragmentSerializer(static fragmentSerializer => fragmentSerializer
                        .Override<TimeSpan>(static (t, env, _, recurse) => recurse.Serialize(t.TotalMilliseconds, env))))
                .ConfigureDeserializer(static deserializer => deserializer
                    .ConfigureFragmentDeserializer(static fragmentDeserializer => fragmentDeserializer
                        .Override<JValue>(static (jValue, type, env, overridden, recurse) => type == typeof(TimeSpan)
                            ? TimeSpan.FromMilliseconds(jValue.Value<double>())
                            : overridden(jValue, type, env, recurse))));
        }

        public static IGremlinQueryEnvironment RegisterNativeType<TNative>(this IGremlinQueryEnvironment environment, GremlinQueryFragmentSerializerDelegate<TNative> serializerDelegate, GremlinQueryFragmentDeserializerDelegate<JValue> deserializer)
        {
            return environment
                .RegisterNativeType(
                    serializerDelegate,
                    _ => _
                        .Override<JValue, TNative>(deserializer));
        }
    }
}
