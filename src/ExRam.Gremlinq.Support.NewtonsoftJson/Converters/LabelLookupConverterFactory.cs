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
            private readonly IGremlinQueryEnvironment _environment;

            public LabelLookupConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JObject serialized, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                // Elements
                var modelTypes = _environment.GetCache().ModelTypesForLabels;
                var label = serialized["label"]?.ToString();

                var modelType = label != null && modelTypes.TryGetValue(label, out var types)
                    ? types.FirstOrDefault(typeof(TTarget).IsAssignableFrom)
                    : default;

                if (modelType != null && modelType != typeof(TTarget))
                {
                    if (recurse.TryDeserialize(modelType).From(serialized, _environment) is TTarget target)
                    {
                        value = target;
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TSource) == typeof(JObject) && !typeof(TSource).IsSealed
                ? (IConverter<TSource, TTarget>)(object)new LabelLookupConverter<TTarget>(environment)
                : default;
        }
    }
}
