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

            bool IConverter<TToken, TTarget?>.TryConvert(TToken serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (serialized.Type == JTokenType.Null)
                {
                    value = null!;
                    return true;
                }

                if (recurse.TryTransform(serialized, _environment, out TTarget requestedValue))
                {
                    value = requestedValue;
                    return true;
                }

                value = null;
                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(JToken).IsAssignableFrom(typeof(TSource)) && typeof(TTarget).IsGenericType && typeof(TTarget).GetGenericTypeDefinition() == typeof(Nullable<>)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NullableConverter<,>).MakeGenericType(typeof(TSource), typeof(TTarget).GetGenericArguments()[0]), environment)
                : null;
        }
    }
}
