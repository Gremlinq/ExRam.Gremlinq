using System.Text;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public sealed class Issue219 : VerifyBase
    {
        public Issue219() : base()
        {

        }

        [Fact]
        public Task Repro()
        {
            var env = GremlinQueryEnvironment.Invalid
                .UseNewtonsoftJson();

            var token = env
                .Deserializer
                .TransformTo<JToken>()
                .From(Encoding.UTF8.GetBytes("\"2021-03-31T14:57:20.3482309Z\""), env);

            return Verify(token.ToString());
        }
    }
}
