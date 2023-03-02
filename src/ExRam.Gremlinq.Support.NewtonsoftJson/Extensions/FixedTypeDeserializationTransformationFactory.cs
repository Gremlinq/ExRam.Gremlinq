using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    internal abstract class FixedTypeConverterFactory<TStaticTarget> : IConverterFactory
        where TStaticTarget : struct
    {
        private sealed class FixedTypeConverter : IConverter<JValue, TStaticTarget>
        {
            private readonly FixedTypeConverterFactory<TStaticTarget> _factory;

            public FixedTypeConverter(FixedTypeConverterFactory<TStaticTarget> factory)
            {
                _factory = factory;
            }

            public bool TryConvert(JValue serialized, IGremlinQueryEnvironment environment, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TStaticTarget value)
            {
                if (_factory.Convert(serialized, environment, defer, recurse) is { } requested)
                {
                    value = requested;

                    return true;
                }

                value = default;

                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TTarget) == typeof(TStaticTarget) && typeof(TSource) == typeof(JValue)
                ? (IConverter<TSource, TTarget>)(object)new FixedTypeConverter(this)
                : null;
        }

        protected abstract TStaticTarget? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer defer, ITransformer recurse);
    }
}
