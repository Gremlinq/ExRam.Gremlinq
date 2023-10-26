using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal static class TypeExtensions
    {
        public static IEnumerable<Type> GetTypeHierarchy(this Type type)
        {
            var currentType = (Type?)type;

            while (currentType != null)
            {
                yield return currentType;
                currentType = currentType.BaseType;
            }
        }

        public static IEnumerable<PropertyInfo> GetSerializableProperties(this Type type)
        {
            return type
                .GetTypeHierarchy()
                .SelectMany(static type => type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                .Where(static p => p.GetMethod is { } getMethod && getMethod.GetBaseDefinition() == getMethod);
        }
    }
}
