using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class ByteArrayToJTokenDeferralConverterFactory : IConverterFactory
    {
        private sealed class ByteArrayToJTokenDeferralConverter<TTarget> : IConverter<ReadOnlyMemory<byte>, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public ByteArrayToJTokenDeferralConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(ReadOnlyMemory<byte> source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                var stream = default(Stream?);

                if (MemoryMarshal.TryGetArray(source, out var segment) && segment is { Array: { } array })
                    stream = new MemoryStream(array, segment.Offset, segment.Count);
                else
                    stream = new MemoryStream(source.ToArray());

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

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(ReadOnlyMemory<byte>)
           ? (IConverter<TSource, TTarget>)(object)new ByteArrayToJTokenDeferralConverter<TTarget>(environment)
           : default;
    }
}
