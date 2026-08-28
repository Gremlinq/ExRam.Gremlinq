using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class KeyConverterFactory : IConverterFactory
    {
        private sealed class KeyConverter<TSource> : IConverter<TSource, Key>
            where TSource : JToken
        {
            bool IConverter<TSource, Key>.TryConvert(TSource serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out Key value)
            {
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                return serialized.TryParseKey(out value);
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(JToken).IsAssignableFrom(typeof(TSource)) && typeof(TTarget) == typeof(Key)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(KeyConverter<>).MakeGenericType(typeof(TSource)))
                : null;
        }
    }
}
