using System.Collections.Frozen;
using System.Reflection;
using ExRam.Gremlinq.Core.Models;

namespace ExRam.Gremlinq.Core
{
    internal interface ICachingGremlinQueryEnvironment : IGremlinQueryEnvironment
    {
        MemberMetadata GetMetadata(MemberInfo member);
        (PropertyInfo propertyInfo, MemberMetadata metadata)[] GetSerializationData(Type type);

        FrozenSet<Type> ModelTypes { get; }
    }
}
