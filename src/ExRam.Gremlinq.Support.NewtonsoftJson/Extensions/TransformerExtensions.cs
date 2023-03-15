using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using Gremlin.Net.Driver.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    internal static class TransformerExtensions
    {
        private static readonly JsonSerializer jsonSerializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            });

        private static readonly ConditionalWeakTable<ITransformer, ITransformer> ShortcutTransformers = new();

        public static ITransformer UseNewtonsoftJson(this ITransformer transformer)
        {
            return transformer
                .Add(ConverterFactory
                    .Create<byte[], ResponseMessage<List<object>>>((message, env, recurse) =>
                    {
                        var token = jsonSerializer
                            .Deserialize<JToken>(new JsonTextReader(new StreamReader(new MemoryStream(message))));

                        return ShortcutTransformers
                            .GetValue(
                                recurse,
                                transformer => transformer
                                    .Add(ConverterFactory
                                        .Create<JToken, JToken>((token, env, recurse) => token)))
                            .TransformTo<ResponseMessage<List<object>>>()
                            .From(token, env);
                    }))
                .Add(new NewtonsoftJsonSerializerConverterFactory())
                .Add(new VertexOrEdgeConverterFactory())
                .Add(new SingleItemArrayFallbackConverterFactory())
                .Add(new PropertyConverterFactory())
                .Add(new ExpandoObjectConverterFactory())  //TODO: Move
                .Add(new LabelLookupConverterFactory())
                .Add(new VertexPropertyExtractConverterFactory())
                .Add(new TypedValueConverterFactory())
                .Add(new ConvertMapsConverterFactory())
                .Add(new BulkSetConverterFactory())
                .Add(new EnumerableConverterFactory())
                .Add(new NullableConverterFactory())
                .Add(new NativeTypeConverterFactory())
                .Add(new TimeSpanConverterFactory())
                .Add(new DateTimeOffsetConverterFactory())
                .Add(new DateTimeConverterFactory());
        }
    }
}
