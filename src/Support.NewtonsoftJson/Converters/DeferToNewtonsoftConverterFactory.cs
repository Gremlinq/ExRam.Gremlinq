using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

using Newtonsoft.Json;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DeferToNewtonsoftConverterFactory : IConverterFactory
    { 
        private sealed class DeferToNewtonsoftConverter<TTarget> : IConverter<ReadOnlyMemory<byte>, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public DeferToNewtonsoftConverter(IGremlinQueryEnvironment environment)
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
                            if (_environment.GetJsonSerializer(recurse).Deserialize<TTarget>(jsonTextReader) is { } requestedValue)
                            {
                                value = requestedValue;
                                return true;
                            }
                        }
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(ReadOnlyMemory<byte>)
            ? (IConverter<TSource, TTarget>)(object)new DeferToNewtonsoftConverter<TTarget>(environment)
            : default;
    }
}
