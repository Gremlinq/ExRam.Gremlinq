using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class NativeTypeConverterFactory : IConverterFactory
    {
        public sealed class NativeTypeConverter<TTarget> : IConverter<JValue, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public NativeTypeConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            bool IConverter<JValue, TTarget>.TryConvert(JValue serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                return recurse.TryTransform(serialized.Value, _environment, out value);
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(JValue).IsAssignableFrom(typeof(TSource))
                ? Unsafe.As<IConverter<TSource, TTarget>>(new NativeTypeConverter<TTarget>(environment))
                : null;
        }
    }
}
