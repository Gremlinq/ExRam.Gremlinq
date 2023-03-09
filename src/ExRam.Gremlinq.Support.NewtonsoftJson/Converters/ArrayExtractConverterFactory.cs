using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Collections;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class ArrayExtractConverterFactory : IConverterFactory
    {
        private sealed class ArrayExtractConverter<TTarget> : IConverter<JArray, TTarget>
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
            return typeof(TSource) == typeof(JArray)
                ? (IConverter<TSource, TTarget>)(object)new ArrayExtractConverter<TTarget>()
                : default;
        }
    }
}
