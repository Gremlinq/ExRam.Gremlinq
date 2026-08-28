using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class BulkSetConverterFactory : IConverterFactory
    {
        private sealed class BulkSetConverter<TTargetArray, TTargetArrayItem> : IConverter<JObject, TTargetArray>
            where TTargetArray : class
        {
            private readonly IGremlinQueryEnvironment _environment;

            public BulkSetConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            bool IConverter<JObject, TTargetArray>.TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (serialized.TryGetValue("@type", out var typeToken) && "g:BulkSet".Equals(typeToken.Value<string>(), StringComparison.OrdinalIgnoreCase))
                {
                    if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray setArray)
                    {
                        var array = new List<TTargetArrayItem>(setArray.Count);

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

                        value = Unsafe.As<TTargetArray>(array.ToArray());
                        return true;
                    }
                }

                value = null;
                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            if (typeof(TSource) == typeof(JObject) && !environment.SupportsType(typeof(TTarget)))
            {
                var elementType = typeof(TTarget).GetElementType() ?? typeof(object);
                var arrayType = elementType.MakeArrayType();

                if (typeof(TTarget).IsAssignableFrom(arrayType)) 
                    return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(BulkSetConverter<,>).MakeGenericType(typeof(TTarget), elementType), environment);
            }

            return null;
        }
    }
}
