using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class NullableConverterFactory : IConverterFactory
    {
        private sealed class NullableConverter<TTarget> : IConverter<JToken, TTarget?>
            where TTarget : struct
        {
            public bool TryConvert(JToken serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (serialized.Type == JTokenType.Null)
                {
                    value = default(TTarget);
                    return true;
                }

                if (recurse.TryTransform<JToken, TTarget>(serialized, environment, out var requestedValue))
                {
                    value = requestedValue;
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TSource) == typeof(JToken) && typeof(TTarget).IsGenericType && typeof(TTarget).GetGenericTypeDefinition() == typeof(Nullable<>)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NullableConverter<>).MakeGenericType(typeof(TTarget).GetGenericArguments()[0]))
                : default;
        }
    }
}
