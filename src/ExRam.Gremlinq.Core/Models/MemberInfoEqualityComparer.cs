using System.Collections.Generic;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal sealed class MemberInfoEqualityComparer : IEqualityComparer<MemberInfo>
    {
        internal sealed class InnerMemberInfoEqualityComparer : IEqualityComparer<MemberInfo>
        {
            public static readonly InnerMemberInfoEqualityComparer Instance = new InnerMemberInfoEqualityComparer();

            public bool Equals(MemberInfo? x, MemberInfo? y)
            {
                return (x?.DeclaringType, x?.MetadataToken).Equals((y?.DeclaringType, y?.MetadataToken));
            }

            public int GetHashCode(MemberInfo obj)
            {
                return (obj.DeclaringType, obj.MetadataToken).GetHashCode();
            }
        }

        public static readonly MemberInfoEqualityComparer Instance = new MemberInfoEqualityComparer();

        private MemberInfoEqualityComparer()
        {
            
        }

        public bool Equals(MemberInfo? x, MemberInfo? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return InnerMemberInfoEqualityComparer.Instance.Equals(GetBaseMemberInfo(x), GetBaseMemberInfo(y));
        }

        public int GetHashCode(MemberInfo obj)
        {
            return InnerMemberInfoEqualityComparer.Instance.GetHashCode(GetBaseMemberInfo(obj));
        }

        private static MemberInfo GetBaseMemberInfo(MemberInfo member)
        {
            if (member is PropertyInfo propertyInfo)
            {
                if (propertyInfo.GetMethod is { } interfaceGetter)
                {
                    if (member.DeclaringType != null && !member.DeclaringType.IsInterface)
                    {
                        foreach (var iface in member.DeclaringType.GetInterfaces())
                        {
                            var interfaceMap = member.DeclaringType
                                .GetInterfaceMap(iface);

                            for (var i = 0; i < interfaceMap.TargetMethods.Length; i++)
                            {
                                if (InnerMemberInfoEqualityComparer.Instance.Equals(interfaceMap.TargetMethods[i], interfaceGetter))
                                    return interfaceMap.InterfaceMethods[i];
                            }
                        }
                    }

                    return interfaceGetter;
                }
            }

            return member;
        }
    }
}
