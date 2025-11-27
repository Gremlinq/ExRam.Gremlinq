using ExRam.Gremlinq.Tests.Infrastructure;

using FluentAssertions;
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

        protected override JToken CreateNativeToken(string str) => JToken.Parse(str);
    }
}
