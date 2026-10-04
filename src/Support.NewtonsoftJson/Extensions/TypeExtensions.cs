namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal static class TypeExtensions
    {
        // The type arguments of the closed genericTypeDefinition a type is or derives from, so that
        // a type of the caller's own that derives from Property<TValue> is a property of TValue.
        public static Type[]? TryGetGenericArgumentsOf(this Type type, Type genericTypeDefinition)
        {
            for (var currentType = type; currentType is not null; currentType = currentType.BaseType)
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == genericTypeDefinition)
                    return currentType.GetGenericArguments();
            }

            return null;
        }
    }
}
