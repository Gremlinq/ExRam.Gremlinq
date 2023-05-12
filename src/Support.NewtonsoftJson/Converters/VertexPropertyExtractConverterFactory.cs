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
            private readonly IGremlinQueryEnvironment _environment;

            public VertexPropertyExtractConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JToken serialized, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                var isNativeType = _environment.SupportsType(typeof(TTarget)) || typeof(TTarget).IsEnum && _environment.SupportsType(typeof(TTarget).GetEnumUnderlyingType());

                if (serialized is JObject jObject && !typeof(Property).IsAssignableFrom(typeof(TTarget)))
                {
                    if (jObject.LooksLikeVertexProperty() && typeof(TTarget).IsAssignableFrom(typeof(VertexProperty<object>)))
                    {
                        if (recurse.TryTransform(jObject, _environment, out VertexProperty<object>? vProp) && vProp is TTarget target)
                        {
                            value = target;
                            return true;
                        }
                    }
                    else if (jObject.LooksLikeProperty() && typeof(TTarget).IsAssignableFrom(typeof(Property<object>)))
                    {
                        if (recurse.TryTransform(jObject, _environment, out Property<object>? prop) && prop is TTarget target)
                        {
                            value = target;
                            return true;
                        }
                    }

                    if (isNativeType && jObject.TryGetValue("value", out var valueToken) && recurse.TryTransform(valueToken, _environment, out value))
                        return true;
                }
                else if (serialized is JArray { Count: 1 } jArray)
                    return recurse.TryTransform(jArray[0], _environment, out value);

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(JToken).IsAssignableFrom(typeof(TSource))
            ? (IConverter<TSource, TTarget>)(object)new VertexPropertyExtractConverter<TTarget>(environment)
            : default;
    }
}
