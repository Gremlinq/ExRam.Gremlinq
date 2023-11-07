﻿using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core.Deserialization
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

            public bool TryConvert(byte[] source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                return recurse.TryTransform((ReadOnlyMemory<byte>)source.AsMemory(), _environment, out value);
            }
        }

        private sealed class DeferFromMemoryConverter<TTarget> : IConverter<Memory<byte>, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public DeferFromMemoryConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(Memory<byte> source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                return recurse.TryTransform((ReadOnlyMemory<byte>)source, _environment, out value);
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            if (typeof(TSource) == typeof(byte[]))
                return (IConverter<TSource, TTarget>)(object)new DeferFromByteArrayConverter<TTarget>(environment);

            if (typeof(TSource) == typeof(Memory<byte>))
                return (IConverter<TSource, TTarget>)(object)new DeferFromMemoryConverter<TTarget>(environment);

            return default;
        }
    }
}
