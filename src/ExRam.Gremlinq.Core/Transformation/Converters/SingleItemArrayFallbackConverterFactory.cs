using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class SingleItemArrayFallbackConverterFactory : IConverterFactory
    {
        private sealed class SingleItemArrayFallbackConverter<TSource, TTargetArray, TTargetArrayItem> : IConverter<TSource, TTargetArray>
        {
            public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
            {
                if (recurse.TryTransform<TSource, TTargetArrayItem>(source, environment, out var typedValue))
                {
                    value = (TTargetArray)(object)new[] { typedValue };
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TTarget).IsArray
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackConverter<,,>).MakeGenericType(typeof(TSource), typeof(TTarget), typeof(TTarget).GetElementType()!))
                : default;
        }
    }
}
