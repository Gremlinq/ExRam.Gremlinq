// ReSharper disable ArrangeThisQualifier
// ReSharper disable CoVariantArrayConversion
namespace ExRam.Gremlinq.Core
{
    internal readonly struct FilterTypes
    {
        private readonly Type[]? _types;
        private readonly InvalidOperationException? _exception;

        public static readonly FilterTypes None = new(default(Type[]?));

        private FilterTypes(Type[]? types)
        {
            _types = types;
        }

        private FilterTypes(InvalidOperationException exception)
        {
            _exception = exception;
        }

        public static FilterTypes From(Type[] types) => new(types);

        public static FilterTypes From(InvalidOperationException exception) => new(exception);

        public FilterTypes Sanitize<TBaseType>()
        {
            if (_types is { } types)
            {
                if (types.Any(static type => type.IsAssignableFrom(typeof(TBaseType)) || type == typeof(object)))
                    return None;

                var sanitizedTypes = types
                    .Where(static edgeType => typeof(TBaseType).IsAssignableFrom(edgeType))
                    .ToArray();

                return sanitizedTypes.Length > 0
                    ? From(sanitizedTypes)
                    : From(new InvalidOperationException($"The graph model does not contain any types assignable to any of {string.Join(',', types.Select(type => type.FullName))} in the type hierarchy of {typeof(TBaseType).FullName}."));
            }

            return this;
        }

        public bool IsNone => _exception is null && _types is null;

        public Type[] Types => _exception is { } exception
            ? throw exception
            : _types ?? throw new InvalidOperationException("FilterTypes represents no filter.");
    }
}
