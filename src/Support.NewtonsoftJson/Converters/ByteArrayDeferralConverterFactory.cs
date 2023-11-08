using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

using Newtonsoft.Json.Linq;

using Newtonsoft.Json;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
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

        private sealed class DeferToJTokenConverter<TTarget> : IConverter<ReadOnlyMemory<byte>, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public DeferToJTokenConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(ReadOnlyMemory<byte> source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                var stream = MemoryMarshal.TryGetArray(source, out var segment) && segment is { Array: { } array }
                    ? new MemoryStream(array, segment.Offset, segment.Count)
                    : new MemoryStream(source.ToArray());

                using (stream)
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        using (var jsonTextReader = new JsonTextReader(streamReader) { DateParseHandling = DateParseHandling.None })
                        {
                            var maybeToken = default(JToken?);

                            try
                            {
                                maybeToken = JToken.ReadFrom(jsonTextReader);
                            }
                            catch (JsonException)
                            {

                            }

                            if (maybeToken is { } token)
                                return recurse.TryTransform(token, _environment, out value);
                        }
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            if (typeof(TSource) == typeof(ReadOnlyMemory<byte>))
                return (IConverter<TSource, TTarget>)(object)new DeferToJTokenConverter<TTarget>(environment);

            if (typeof(TSource) == typeof(Memory<byte>))
                return (IConverter<TSource, TTarget>)(object)new DeferFromMemoryConverter<TTarget>(environment);

            if (typeof(TSource) == typeof(byte[]))
                return (IConverter<TSource, TTarget>)(object)new DeferFromByteArrayConverter<TTarget>(environment);

            return default;
        }
    }
}
