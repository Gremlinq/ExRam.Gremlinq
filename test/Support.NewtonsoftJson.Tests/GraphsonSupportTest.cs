using System.Collections;
using System.Text;

using ExRam.Gremlinq.Tests.Infrastructure;

using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public class GraphsonSupportTest : GraphsonSupportTestBase<JToken>, ISourceFileNameProvider<GraphsonSupportTest>
    {
        private readonly struct NativeType
        {
            public NativeType(int value)
            {
                Value = value;
            }

            public int Value { get; }
        }

        private static readonly JsonSerializer JsonSerializer = JsonSerializer.CreateDefault();

        public GraphsonSupportTest() : base(env => env.UseNewtonsoftJson())
        {

        }

        public static string GetSourceFileName() => SourceFileName.OfThis();

        [Fact]
        public void JToken_Load_does_not_reuse()
        {
            var token = GraphSonStrings.Single_Language;

            var readToken1 = JToken.Load(new JTokenReader(token));
            var readToken2 = JToken.Load(new JTokenReader(token));

            readToken1
                .Should()
                .NotBeSameAs(readToken2);
        }

        [Fact]
        public async Task NativeType_is_deserialized()
        {
            var data = "[ 42 ]";

            await Verify<NativeType>(data, env => env
                .RegisterNativeType(
                    (_, _, _, _) => 42,
                    (jValue, _, _, _) => jValue.Type is JTokenType.Integer
                        ? new NativeType(jValue.Value<int>())
                        : default));
        }

        [Fact]
        public async Task NativeType_is_only_deserialized_when_requested_explicitly()
        {
            var data = "[ \"originalString\" ]";

            await Verify<object>(data, env => env
                .RegisterNativeType(
                    (_, _, _, _) => 42,
                    (jValue, _, _, _) => jValue.Type is JTokenType.Integer
                        ? new NativeType(jValue.Value<int>())
                        : default));
        }

        // A typed value is no map, nor a collection of its @type and its @value, and no serializer
        // hands out a token of its own. Newtonsoft still does, until ExRam.Gremlinq#2495 lets it
        // stop Core's transformer from giving the source token as it is: the JObject that holds a
        // typed value is an ICollection, and the JValue of a g:Int32's @value an IEnumerable. These
        // record that answer under a name of their own, next to the declines the contract asks for.
        public override Task IUntypedCollection_from_typed_Int32() => VerifyAttemptWithRuntimeType<ICollection>("""{ "@type": "g:Int32", "@value": 5 }""")
            .UseMethodName($"{nameof(IUntypedCollection_from_typed_Int32)}_until_2495");

        public override Task IUntypedCollection_from_typed_UUID() => VerifyAttemptWithRuntimeType<ICollection>("""{ "@type": "g:UUID", "@value": "41d2e28a-20a4-4ab0-b379-d810dede3786" }""")
            .UseMethodName($"{nameof(IUntypedCollection_from_typed_UUID)}_until_2495");

        public override Task IUntypedCollection_from_typed_T() => VerifyAttemptWithRuntimeType<ICollection>("""{ "@type": "g:T", "@value": "id" }""")
            .UseMethodName($"{nameof(IUntypedCollection_from_typed_T)}_until_2495");

        public override Task Untyped_IEnumerable_from_typed_Int32() => VerifyAttemptWithRuntimeType<IEnumerable>("""{ "@type": "g:Int32", "@value": 5 }""")
            .UseMethodName($"{nameof(Untyped_IEnumerable_from_typed_Int32)}_until_2495");

        // Reads the JSON of a test the way DeferToNewtonsoftConverterFactory reads a response, so
        // that the converters are handed here what they are handed there. JToken.Parse is not
        // that. It leaves DateParseHandling at DateTime, under which a string that looks like a
        // date has become a Date token before any converter has seen it, and one naming an offset
        // has been moved into this machine's time zone on the way - where a response is read with
        // DateParseHandling.None, its strings stay strings, and telling a date from text is left
        // to the converters. The rest is mirrored for the same reason: the reader is otherwise
        // left as it is created (FloatParseHandling.Double, a MaxDepth of 64, the invariant
        // culture), it reads UTF-8 bytes through a StreamReader, and the token is built by a
        // default JsonSerializer rather than by JToken.Load - which keeps a name that occurs
        // twice at its last place instead of its first, keeps a comment as a token, and does not
        // mind text after the token, where JToken.Parse throws.
        protected override JToken CreateNativeToken(string str)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        jsonTextReader.DateParseHandling = DateParseHandling.None;

                        return JsonSerializer.Deserialize<JToken>(jsonTextReader) ?? throw new InvalidOperationException("There is no token to read.");
                    }
                }
            }
        }
    }
}
