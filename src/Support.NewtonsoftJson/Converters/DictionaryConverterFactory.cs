using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DictionaryConverterFactory : IConverterFactory
    {
        private sealed class DictionaryConverter<TTarget> : IConverter<JObject, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public DictionaryConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                var ret = new Dictionary<string, object?>();

                foreach (var property in serialized)
                {
                    if (property.Value is { } propertyValue && recurse.TryTransform(propertyValue, _environment, out object? item))
                        ret.TryAdd(property.Key, item);
                }

                value = (TTarget)(object)ret;
                return true;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TSource) == typeof(JObject) && typeof(TTarget).IsAssignableFrom(typeof(Dictionary<string, object?>))
                ? (IConverter<TSource, TTarget>)(object)new DictionaryConverter<TTarget>(environment)
                : default;
        }
    }
}
