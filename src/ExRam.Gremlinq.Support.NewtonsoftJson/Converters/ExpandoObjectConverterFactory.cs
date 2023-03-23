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
            private readonly IGremlinQueryEnvironment _environment;

            public ExpandoObjectConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (recurse.TryTransform<JObject, JObject>(serialized, _environment, out var strippedJObject))
                {
                    var expando = new ExpandoObject();

                    foreach (var property in strippedJObject)
                        if (property.Value is { } propertyValue && recurse.TryTransform<JToken, object>(propertyValue, _environment, out var item))
                            expando.TryAdd(property.Key, item);

                    value = (TTarget)(object)expando;
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TSource) == typeof(JObject) && typeof(TTarget).IsAssignableFrom(typeof(ExpandoObject))
                ? (IConverter<TSource, TTarget>)(object)new ExpandoObjectConverter<TTarget>(environment)
                : default;
        }
    }
}
