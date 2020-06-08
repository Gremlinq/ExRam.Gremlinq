using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal sealed class MemberInfoEqualityComparer : IEqualityComparer<MemberInfo>
    {
        public static readonly MemberInfoEqualityComparer Instance = new MemberInfoEqualityComparer();

        private MemberInfoEqualityComparer()
        {
            
        }

        public bool Equals(MemberInfo x, MemberInfo y)
        {
            x = GetBaseMemberInfo(x);
            y = GetBaseMemberInfo(y);

            return (x?.DeclaringType, x?.MetadataToken).Equals((y?.DeclaringType, y?.MetadataToken));
        }

        public int GetHashCode(MemberInfo obj)
        {
            obj = GetBaseMemberInfo(obj);

            return (obj.DeclaringType, obj.MetadataToken).GetHashCode();
        }

        private MemberInfo GetBaseMemberInfo(MemberInfo member)
        {
            if (member is PropertyInfo propertyInfo)
            {
                var interfaceGetter = propertyInfo.GetMethod;

                if (member.DeclaringType != null && !member.DeclaringType.IsInterface)
                {
                    foreach (var iface in member.DeclaringType.GetInterfaces())
                    {
                        var interfaceMap = member.DeclaringType
                            .GetInterfaceMap(iface);

                        for (var i = 0; i < interfaceMap.TargetMethods.Length; i++)
                        {
                            if ((interfaceMap.TargetMethods[i].DeclaringType, interfaceMap.TargetMethods[i].MetadataToken) == (interfaceGetter.DeclaringType, interfaceGetter.MetadataToken))
                                return interfaceMap.InterfaceMethods[i];
                        }
                    }
                }

                return interfaceGetter;
            }

            return member;
        }
    }
}
