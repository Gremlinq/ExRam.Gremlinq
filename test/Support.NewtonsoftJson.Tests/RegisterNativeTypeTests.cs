using ExRam.Gremlinq.Core;
using Newtonsoft.Json.Linq;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public class RegisterNativeTypeTests : VerifyBase
    {
        private readonly struct FancyId
        {
            public FancyId(string wrappedId)
            {
                WrappedId = wrappedId;
            }

            public string WrappedId { get; }
        }

        public RegisterNativeTypeTests() : base()
        {
        }

        [Fact]
        public Task Serialization() => Verify(g
            .ConfigureEnvironment(env => env
                .UseNewtonsoftJson()
                .RegisterNativeType(
                    (fancyId, env, _, recurse) => fancyId.WrappedId,
                    (jValue, env, _, recurse) => new FancyId(jValue.Value<string>()!)))
            .Inject(new FancyId("fancyId"))
            .Debug());

        //TODO: DeserializationTests
    }
}
