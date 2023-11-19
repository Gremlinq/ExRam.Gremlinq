using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class ByteArrayDeferralConverterFactory : IConverterFactory
    {
        private sealed class DeferFromByteArrayConverter<TTarget> : IConverter<byte[], TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public DeferFromByteArrayConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(byte[] source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value) => recurse.TryTransform((ReadOnlyMemory<byte>)source.AsMemory(), _environment, out value);
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(byte[])
            ? (IConverter<TSource, TTarget>)(object)new DeferFromByteArrayConverter<TTarget>(environment)
            : default;
    }
}
