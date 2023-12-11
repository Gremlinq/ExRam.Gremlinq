using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.Core;

using Newtonsoft.Json;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DeferToNewtonsoftConverterFactory : IConverterFactory
    { 
        private sealed class DeferToNewtonsoftConverter<TBuffer, TTarget> : IConverter<TBuffer, TTarget>
            where TBuffer : IMessageBuffer
        {
            private readonly IGremlinQueryEnvironment _environment;

            public DeferToNewtonsoftConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(TBuffer source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                var stream = MemoryMarshal.TryGetArray<byte>(source.Memory, out var segment) && segment is { Array: { } array }
                    ? new MemoryStream(array, segment.Offset, segment.Count)
                    : new MemoryStream(source.Memory.ToArray());

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

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(GraphSon2MessageBuffer)
            ? (IConverter<TSource, TTarget>)(object)new DeferToNewtonsoftConverter<GraphSon2MessageBuffer, TTarget>(environment)
            : typeof(TSource) == typeof(GraphSon3MessageBuffer)
                ? (IConverter<TSource, TTarget>)(object)new DeferToNewtonsoftConverter<GraphSon3MessageBuffer, TTarget>(environment)
                : default;
    }
}
