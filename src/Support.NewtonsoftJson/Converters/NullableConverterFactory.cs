using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class NullableConverterFactory : IConverterFactory
    {
        private sealed class NullableConverter<TToken, TTarget> : IConverter<TToken, TTarget?>
            where TToken : JToken
            where TTarget : struct
        {
            private readonly IGremlinQueryEnvironment _environment;

            public NullableConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(TToken serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (serialized.Type == JTokenType.Null)
                {
                    value = default!;
                    return true;
                }

                if (recurse.TryTransform(serialized, _environment, out TTarget requestedValue))
                {
                    value = requestedValue;
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(JToken).IsAssignableFrom(typeof(TSource)) && typeof(TTarget).IsGenericType && typeof(TTarget).GetGenericTypeDefinition() == typeof(Nullable<>)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NullableConverter<,>).MakeGenericType(typeof(TSource), typeof(TTarget).GetGenericArguments()[0]), environment)
                : default;
        }
    }
}
