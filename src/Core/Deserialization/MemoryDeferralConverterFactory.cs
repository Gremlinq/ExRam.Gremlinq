using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class MemoryDeferralConverterFactory : IConverterFactory
    {
        private sealed class DeferFromMemoryConverter<TTarget> : IConverter<Memory<byte>, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public DeferFromMemoryConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(Memory<byte> source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value) => recurse.TryTransform((ReadOnlyMemory<byte>)source, _environment, out value);
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(Memory<byte>)
            ? (IConverter<TSource, TTarget>)(object)new DeferFromMemoryConverter<TTarget>(environment)
            : default;
    }
}
