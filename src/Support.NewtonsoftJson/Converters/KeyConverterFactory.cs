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
            public bool TryConvert(TSource serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out Key value) => serialized.TryParseKey(out value);
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(JToken).IsAssignableFrom(typeof(TSource)) && typeof(TTarget) == typeof(Key)
            ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(KeyConverter<>).MakeGenericType(typeof(TSource)))
            : null;
    }
}
