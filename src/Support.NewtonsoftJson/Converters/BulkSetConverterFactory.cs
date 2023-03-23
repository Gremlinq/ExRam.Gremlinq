using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class BulkSetConverterFactory : IConverterFactory
    {
        private sealed class BulkSetConverter<TTargetArray, TTargetArrayItem> : IConverter<JObject, TTargetArray>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public BulkSetConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
            {
                if (serialized.TryGetValue("@type", out var typeToken) && "g:BulkSet".Equals(typeToken.Value<string>(), StringComparison.OrdinalIgnoreCase))
                {
                    if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray setArray)
                    {
                        var array = new List<TTargetArrayItem>();

                        for (var i = 0; i < setArray.Count; i += 2)
                        {
                            if (recurse.TryTransform<JToken, TTargetArrayItem>(setArray[i], _environment, out var element))
                            {
                                if (recurse.TryTransform<JToken, int>(setArray[i + 1], _environment, out var bulk) && bulk != 1)
                                {
                                    for (var j = 0; j < bulk; j++)
                                    {
                                        array.Add(element);
                                    }
                                }
                                else
                                    array.Add(element);
                            }
                        }

                        value = (TTargetArray)(object)array.ToArray();
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TTarget).IsArray && !environment.SupportsType(typeof(TTarget)) && typeof(TSource) == typeof(JObject)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(BulkSetConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetElementType()!), environment)
                : default;
        }
    }
}
