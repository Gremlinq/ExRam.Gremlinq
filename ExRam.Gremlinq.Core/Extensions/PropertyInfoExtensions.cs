using ExRam.Gremlinq.GraphElements;

namespace System.Reflection
{
    internal static class PropertyInfoExtensions
    {
        public static bool IsElementLabel(this PropertyInfo propertyInfo)
        {
            return typeof(IElement).IsAssignableFrom(propertyInfo.DeclaringType) && propertyInfo.Name == nameof(IElement.Label);
        }
    }
}