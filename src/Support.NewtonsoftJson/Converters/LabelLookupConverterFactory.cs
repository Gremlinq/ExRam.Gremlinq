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

            public LabelLookupConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;

                _modelTypesForLabels = ModelTypesForLabels.GetValue(
                    environment,
                    static environment => environment.Model
                        .VerticesModel
                        .ElementTypes
                        .Select(type => (Type: type, environment.Model.VerticesModel.GetMetadata(type).Label))
                        .Concat(environment.Model
                            .EdgesModel
                            .ElementTypes
                            .Select(type => (Type: type, environment.Model.EdgesModel.GetMetadata(type).Label)))
                        .GroupBy(static x => x.Label)
                        .ToDictionary(
                            static group => group.Key,
                            static group => group
                                .Select(static x => x.Type)
                                .ToArray(),
                            StringComparer.OrdinalIgnoreCase));
            }

            public bool TryConvert(JObject serialized, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (serialized.LooksLikeElement(out _, out var labelValue, out _))
                {
                    if (labelValue.Value<string>() is { } label && _modelTypesForLabels.TryGetValue(label, out var types) && types.FirstOrDefault(typeof(TTarget).IsAssignableFrom) is { } modelType)
                    {
                        if (modelType != typeof(TTarget) && recurse.TryTransformTo(modelType).From(serialized, _environment) is TTarget target)
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

        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, IReadOnlyDictionary<string, Type[]>> ModelTypesForLabels = new();

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(JObject) && !typeof(TTarget).IsSealed
            ? (IConverter<TSource, TTarget>)(object)new LabelLookupConverter<TTarget>(environment)
            : default;
    }
}
