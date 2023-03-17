using System.Reflection;
using ExRam.Gremlinq.Core.Models;

namespace ExRam.Gremlinq.Core
{
    internal interface IGremlinQueryEnvironmentCache
    {
        Key GetKey(MemberInfo member);
        (PropertyInfo propertyInfo, Key key, SerializationBehaviour serializationBehaviour)[] GetSerializationData(Type type);

        HashSet<Type> ModelTypes { get; }
        IReadOnlyDictionary<string, Type[]> ModelTypesForLabels { get; }
    }
}
