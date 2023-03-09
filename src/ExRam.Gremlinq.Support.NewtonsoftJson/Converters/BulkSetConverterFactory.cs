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
            public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
            {
                if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TTargetArray)))
                    if (serialized.TryGetValue("@type", out var typeToken) && "g:BulkSet".Equals(typeToken.Value<string>(), StringComparison.OrdinalIgnoreCase))
                        if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray setArray)
                        {
                            var array = new List<TTargetArrayItem>();

                            for (var i = 0; i < setArray.Count; i += 2)
                                if (recurse.TryTransform<JToken, TTargetArrayItem>(setArray[i], environment, out var element))
                                    if (recurse.TryTransform<JToken, int>(setArray[i + 1], environment, out var bulk) && bulk != 1)
                                        for (var j = 0; j < bulk; j++)
                                            array.Add(element);
                                    else
                                        array.Add(element);

                            value = (TTargetArray)(object)array.ToArray();
                            return true;
                        }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TTarget).IsArray && typeof(TSource) == typeof(JObject)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(BulkSetConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetElementType()!))
                : default;
        }
    }
}
