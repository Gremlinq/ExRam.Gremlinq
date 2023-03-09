using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class ExpandoObjectConverterFactory : IConverterFactory
    {
        private sealed class ExpandoObjectConverter<TTarget> : IConverter<JObject, TTarget>
        {
            public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (recurse.TryTransform<JObject, JObject>(serialized, environment, out var strippedJObject))
                {
                    var expando = new ExpandoObject();

                    foreach (var property in strippedJObject)
                        if (property.Value is { } propertyValue && recurse.TryTransform<JToken, object>(propertyValue, environment, out var item))
                            expando.TryAdd(property.Key, item);

                    value = (TTarget)(object)expando;
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TSource) == typeof(JObject) && typeof(TTarget).IsAssignableFrom(typeof(ExpandoObject))
                ? (IConverter<TSource, TTarget>)(object)new ExpandoObjectConverter<TTarget>()
                : default;
        }
    }
}
