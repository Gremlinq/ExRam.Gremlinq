using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal abstract class FixedTypeConverterFactory<TStaticTarget> : IConverterFactory
        where TStaticTarget : struct
    {
        private sealed class FixedTypeConverter : IConverter<JValue, TStaticTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;
            private readonly FixedTypeConverterFactory<TStaticTarget> _factory;

            public FixedTypeConverter(FixedTypeConverterFactory<TStaticTarget> factory, IGremlinQueryEnvironment environment)
            {
                _factory = factory;
                _environment = environment;
            }

            bool IConverter<JValue, TStaticTarget>.TryConvert(JValue serialized, ITransformer defer, ITransformer recurse, out TStaticTarget value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (_factory.Convert(serialized, _environment, recurse) is { } requested)
                {
                    value = requested;

                    return true;
                }

                value = default;

                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(TTarget) == typeof(TStaticTarget) && typeof(TSource) == typeof(JValue)
                ? Unsafe.As<IConverter<TSource, TTarget>>(new FixedTypeConverter(this, environment))
                : null;
        }

        protected abstract TStaticTarget? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer recurse);
    }
}
