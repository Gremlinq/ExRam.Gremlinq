using System.Reflection;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Models;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core
{
    internal interface IGremlinQueryEnvironmentCache
    {
        JsonSerializer GetJsonSerializer(IGremlinQueryFragmentDeserializer fragmentDeserializer);

        Key GetKey(MemberInfo member);
        (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[] GetSerializationData(Type type);

        IReadOnlyDictionary<Type, object?> FastNativeTypes { get; }
        IReadOnlyDictionary<string, Type[]> ModelTypesForLabels { get; }
    }
}
