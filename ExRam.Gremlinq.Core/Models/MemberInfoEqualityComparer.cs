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
            return (x?.DeclaringType, x?.Name).Equals((y?.DeclaringType, y?.Name));
        }

        public int GetHashCode(MemberInfo obj)
        {
            return (obj.DeclaringType, obj?.Name).GetHashCode();
        }
    }
}
