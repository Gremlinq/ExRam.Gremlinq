using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class ArrayLiftingConverterFactory : IConverterFactory
    {
        private sealed class ArrayLiftingConverter<TTarget> : IConverter<JArray, TTarget>
        {
            public bool TryConvert(JArray serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (recurse.TryTransform<JArray, object[]>(serialized, environment, out var requested))
                {
                    value = (TTarget)(object)requested;
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TSource) == typeof(JArray) && typeof(TTarget).IsAssignableFrom(typeof(object[]))
                ? (IConverter<TSource, TTarget>)(object)new ArrayLiftingConverter<TTarget>()
                : default;
        }
    }
}
