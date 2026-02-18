using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DictionaryConverterFactory : IConverterFactory
    {
        private sealed class DictionaryConverter<TTarget> : IConverter<JObject, TTarget>
            where TTarget : class
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

                value = Unsafe.As<TTarget>(ret);
                return true;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(JObject) && typeof(TTarget).IsAssignableFrom(typeof(Dictionary<string, object?>))
            ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(DictionaryConverter<>).MakeGenericType(typeof(TTarget)), environment)
            : null;
    }
}
