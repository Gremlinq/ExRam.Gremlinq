using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LanguageExt
{
    internal static class ObjectExtensions
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypeProperties = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static IEnumerable<(PropertyInfo, object)> Serialize(this object obj)
        {
            var propertyInfos = TypeProperties
                .GetOrAdd(
                    obj.GetType(),
                    type => type
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                        .ToArray());

            foreach (var propertyInfo in propertyInfos)
            {
                var value = propertyInfo.GetValue(obj);
                if (value != null)
                    yield return (propertyInfo, value);
            }
        }
    }
}
