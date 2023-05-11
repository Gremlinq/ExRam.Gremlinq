using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class LabelLookupConverterFactory : IConverterFactory
    {
        private sealed class LabelLookupConverter<TTarget> : IConverter<JObject, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;
            private readonly IReadOnlyDictionary<string, Type[]> _modelTypesForLabels;

            private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, IReadOnlyDictionary<string, Type[]>> ModelTypesForLabels = new ();

            public LabelLookupConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;

                _modelTypesForLabels = ModelTypesForLabels.GetValue(
                    environment,
                    static environment => environment.Model
                        .VerticesModel
                        .Metadata
                        .Concat(environment.Model
                            .EdgesModel
                            .Metadata)
                        .GroupBy(static x => x.Value.Label)
                        .ToDictionary(
                            static group => group.Key,
                            static group => group
                                .Select(static x => x.Key)
                                .ToArray(),
                            StringComparer.OrdinalIgnoreCase));
            }

            public bool TryConvert(JObject serialized, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (!serialized.ContainsKey("value"))
                {
                    // Elements
                    var label = serialized["label"]?.ToString();

                    var modelType = label != null && _modelTypesForLabels.TryGetValue(label, out var types)
                        ? types.FirstOrDefault(typeof(TTarget).IsAssignableFrom)
                        : default;

                    if (modelType != null && modelType != typeof(TTarget))
                    {
                        if (recurse.TryTransformTo(modelType).From(serialized, _environment) is TTarget target)
                        {
                            value = target;
                            return true;
                        }
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TSource) == typeof(JObject) && !typeof(TTarget).IsSealed
                ? (IConverter<TSource, TTarget>)(object)new LabelLookupConverter<TTarget>(environment)
                : default;
        }
    }
}
