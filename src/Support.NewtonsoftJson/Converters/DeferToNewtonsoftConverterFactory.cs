using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.Core;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DeferToNewtonsoftConverterFactory : IConverterFactory
    {
        private static readonly JsonSerializer JsonSerializer = JsonSerializer.CreateDefault();

        private static class DeferToNewtonsoftConverter<TBinaryMessage>
            where TBinaryMessage : IMemoryOwner<byte>
        {
            private static readonly ThreadLocal<(TBinaryMessage, JToken)?> LastSerialization = new();

            public sealed class DeferToNewtonsoftConverterImpl<TTarget> : IConverter<TBinaryMessage, TTarget>
            {
                private readonly IGremlinQueryEnvironment _environment;

                public DeferToNewtonsoftConverterImpl(IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                }

                public bool TryConvert(TBinaryMessage source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (LastSerialization.Value is { Item1: { } lastMessage, Item2: { } lastToken } && EqualityComparer<TBinaryMessage>.Default.Equals(lastMessage, source))
                        return recurse.TryTransform(lastToken, _environment, out value);

                    var stream = MemoryMarshal.TryGetArray<byte>(source.Memory, out var segment) && segment is { Array: { } array }
                        ? new MemoryStream(array, segment.Offset, segment.Count)
                        : new MemoryStream(source.Memory.ToArray());

                    using (stream)
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            using (var jsonTextReader = new JsonTextReader(streamReader) { DateParseHandling = DateParseHandling.None })
                            {
                                if (JsonSerializer.Deserialize<JToken>(jsonTextReader) is { } jToken)
                                {
                                    LastSerialization.Value = (source, jToken);

                                    return recurse.TryTransform(jToken, _environment, out value);
                                }
                            }
                        }
                    }

                    value = default;
                    return false;
                }
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(GraphSon2BinaryMessage)
            ? (IConverter<TSource, TTarget>)(object)new DeferToNewtonsoftConverter<GraphSon2BinaryMessage>.DeferToNewtonsoftConverterImpl<TTarget>(environment)
            : typeof(TSource) == typeof(GraphSon3BinaryMessage)
                ? (IConverter<TSource, TTarget>)(object)new DeferToNewtonsoftConverter<GraphSon3BinaryMessage>.DeferToNewtonsoftConverterImpl<TTarget>(environment)
                : default;
    }
}
