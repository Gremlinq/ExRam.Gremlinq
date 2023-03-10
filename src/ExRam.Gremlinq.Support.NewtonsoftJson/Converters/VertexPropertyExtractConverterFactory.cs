using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

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

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TSource) == typeof(JObject)
                ? (IConverter<TSource, TTarget>)(object)new VertexPropertyExtractConverter<TTarget>()
                : default;
        }
    }
}
