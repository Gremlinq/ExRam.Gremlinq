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

            public bool TryConvert(JValue serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value) => recurse.TryTransform(serialized.Value, _environment, out value);
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(JValue).IsAssignableFrom(typeof(TSource))
            ? Unsafe.As<IConverter<TSource, TTarget>>(new NativeTypeConverter<TTarget>(environment))
            : null;
    }
}
