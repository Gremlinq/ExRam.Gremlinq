using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using Newtonsoft.Json;
using System.Collections;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class VertexPropertyExtractConverterFactory : IConverterFactory
    {
        private sealed class VertexPropertyExtractConverter<TTarget> : IConverter<JObject, TTarget>
        {
            public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                var nativeTypes = environment.GetCache().FastNativeTypes;

                if (nativeTypes.ContainsKey(typeof(TTarget)) || typeof(TTarget).IsEnum && nativeTypes.ContainsKey(typeof(TTarget).GetEnumUnderlyingType()))
                {
                    if (serialized.TryGetValue("value", out var valueToken))
                    {
                        if (recurse.TryTransform(valueToken, environment, out value))
                            return true;
                    }
                }

                value = default;
                return false;
            }
        }

        private sealed class VertexPropertyArrayExtractConverter<TTarget> : IConverter<JArray, TTarget>
        {
            public bool TryConvert(JArray serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if ((!typeof(IEnumerable).IsAssignableFrom(typeof(TTarget)) || environment.GetCache().FastNativeTypes.ContainsKey(typeof(TTarget))) && !typeof(TTarget).IsInstanceOfType(serialized))
                {
                    if (serialized.Count != 1)
                    {
                        value = serialized.Count == 0 && typeof(TTarget).IsClass
                            ? default!  //TODO: Drop NotNullWhen(true) ?
                            : throw new JsonReaderException($"Cannot convert array\r\n\r\n{serialized}\r\n\r\nto scalar value of type {typeof(TTarget)}.");

                        return true;
                    }

                    return recurse.TryTransform(serialized[0], environment, out value);
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            if (typeof(TSource) == typeof(JObject))
                return (IConverter<TSource, TTarget>)(object)new VertexPropertyExtractConverter<TTarget>();

            if (typeof(TSource) == typeof(JArray))
                return (IConverter<TSource, TTarget>)(object)new VertexPropertyArrayExtractConverter<TTarget>();

            return default;
        }
    }
}
