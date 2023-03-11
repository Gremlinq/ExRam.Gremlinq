using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class VertexPropertyExtractConverterFactory : IConverterFactory
    {
        private sealed class VertexPropertyExtractConverter<TTarget> : IConverter<JToken, TTarget>
        {
            public bool TryConvert(JToken serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                var nativeTypes = environment.GetCache().FastNativeTypes;
                var isNativeType = nativeTypes.ContainsKey(typeof(TTarget)) || typeof(TTarget).IsEnum && nativeTypes.ContainsKey(typeof(TTarget).GetEnumUnderlyingType());

                if (serialized is JObject jObject)
                {
                    if (isNativeType && jObject.TryGetValue("value", out var valueToken) && recurse.TryTransform(valueToken, environment, out value))
                        return true;
                }
                else if (serialized is JArray { Count: 1 } jArray)
                {
                    if (isNativeType || (typeof(TTarget).IsConstructedGenericType && (typeof(TTarget).GetGenericTypeDefinition() == typeof(VertexProperty<>) || typeof(TTarget).GetGenericTypeDefinition() == typeof(VertexProperty<,>))))
                        return recurse.TryTransform(jArray[0], environment, out value);
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>() => typeof(JToken).IsAssignableFrom(typeof(TSource))
            ? (IConverter<TSource, TTarget>)(object)new VertexPropertyExtractConverter<TTarget>()
            : default;
    }
}
