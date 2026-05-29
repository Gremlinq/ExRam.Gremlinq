using System.Reflection;
using ExRam.Gremlinq.Core.Models;

namespace ExRam.Gremlinq.Core
{
    internal interface ICachingGremlinQueryEnvironment : IGremlinQueryEnvironment
    {
        IGremlinQueryEnvironment InnerEnvironment { get; }

        MemberMetadata GetMetadata(MemberInfo member);
        (PropertyInfo propertyInfo, MemberMetadata metadata)[] GetSerializationData(Type type);

        HashSet<Type> ModelTypes { get; }
    }
}
