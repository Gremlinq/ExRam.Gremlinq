using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    internal static class TypeExtensions
    {
        public static IEnumerable<Type> GetTypeHierarchy(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}
