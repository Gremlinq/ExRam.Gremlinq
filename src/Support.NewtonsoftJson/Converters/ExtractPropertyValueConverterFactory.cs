using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class ExtractPropertyValueConverterFactory : IConverterFactory
    {
        private sealed class ExtractPropertyValueConverter<TTarget> : IConverter<JToken, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public ExtractPropertyValueConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JToken serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (serialized is JObject jObject)
                {
                    if (!typeof(Property).IsAssignableFrom(typeof(TTarget)) && (jObject.LooksLikeProperty() || jObject.LooksLikeVertexProperty()) && jObject.TryGetValue("value", out var valueToken))
                        return recurse.TryTransform(valueToken, _environment, out value);
                }
                else if (serialized is JArray { Count: 1 } jArray)
                    return recurse.TryTransform(jArray[0], _environment, out value);

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(JToken).IsAssignableFrom(typeof(TSource))
            ? (IConverter<TSource, TTarget>)(object)new ExtractPropertyValueConverter<TTarget>(environment)
            : default;
    }
}
