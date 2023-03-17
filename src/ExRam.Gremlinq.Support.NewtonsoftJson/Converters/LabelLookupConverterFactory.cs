using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class LabelLookupConverterFactory : IConverterFactory
    {
        private sealed class LabelLookupConverter<TTarget> : IConverter<JObject, TTarget>
        {
            public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                // Elements
                var modelTypes = environment.GetCache().ModelTypesForLabels;
                var label = serialized["label"]?.ToString();

                var modelType = label != null && modelTypes.TryGetValue(label, out var types)
                    ? types.FirstOrDefault(typeof(TTarget).IsAssignableFrom)
                    : default;

                if (modelType != null && modelType != typeof(TTarget))
                {
                    if (recurse.TryDeserialize(modelType).From(serialized, environment) is TTarget target)
                    {
                        value = target;
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TSource) == typeof(JObject) && !typeof(TSource).IsSealed
                ? (IConverter<TSource, TTarget>)(object)new LabelLookupConverter<TTarget>()
                : default;
        }
    }
}
