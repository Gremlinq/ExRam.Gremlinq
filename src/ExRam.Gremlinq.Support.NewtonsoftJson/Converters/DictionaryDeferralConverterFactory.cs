using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DictionaryDeferralConverterFactory : IConverterFactory
    {
        private sealed class DictionaryDeferralConverter<TTarget> : IConverter<JObject, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public DictionaryDeferralConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JObject serialized, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (recurse.TryTransform<JObject, IDictionary<string, object>>(serialized, _environment, out var dict))
                {
                    value = (TTarget)dict;
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TSource) == typeof(JObject) && typeof(TTarget) != typeof(IDictionary<string, object>) && typeof(TTarget).IsAssignableFrom(typeof(IDictionary<string, object>))
                ? (IConverter<TSource, TTarget>)(object)new DictionaryDeferralConverter<TTarget>(environment)
                : default;
        }
    }
}
