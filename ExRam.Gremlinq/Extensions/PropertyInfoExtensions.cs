using ExRam.Gremlinq.GraphElements;

namespace System.Reflection
{
    internal static class PropertyInfoExtensions
    {
        public static bool IsElementLabel(this PropertyInfo propertyInfo)
        {
            return propertyInfo.DeclaringType == typeof(Element) && propertyInfo.Name == nameof(Element.Label);
        }
    }
}