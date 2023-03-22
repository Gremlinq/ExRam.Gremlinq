using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class SingleItemArrayFallbackConverterFactory : IConverterFactory
    {
        private sealed class SingleItemArrayFallbackConverter<TSource, TTargetArray, TTargetArrayItem> : IConverter<TSource, TTargetArray>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public SingleItemArrayFallbackConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(TSource source, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
            {
                if (recurse.TryTransform<TSource, TTargetArrayItem>(source, _environment, out var typedValue))
                {
                    value = (TTargetArray)(object)new[] { typedValue };
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TTarget).IsArray
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackConverter<,,>).MakeGenericType(typeof(TSource), typeof(TTarget), typeof(TTarget).GetElementType()!), environment)
                : default;
        }
    }
}
