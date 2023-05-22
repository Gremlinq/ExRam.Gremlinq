using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public class Issue219 : GremlinqTestBase
    {
        public Issue219(ITestOutputHelper testOutputHelper) : base(GremlinqTestFixture.Empty, GremlinQueryVerifier.Default, testOutputHelper)
        {

        }

        [Fact]
        public Task Repro()
        {
            var env = GremlinQueryEnvironment.Empty
                .UseNewtonsoftJson();

            var token = env
                .Deserializer
                .TransformTo<JToken>()
                .From("\"2021-03-31T14:57:20.3482309Z\"", env);

            return Verify(token.ToString());
        }
    }
}
