using System;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryFragmentDeserializerTest : GremlinqTestBase
    {
        public GremlinQueryFragmentDeserializerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public async Task Empty()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Base_type()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<object>((serialized, type, env, overridden, recurse) => "overridden")
                .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Irrelevant()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<JObject>((serialized, type, env, overridden, recurse) => "should not be here")
                .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override1()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<string>((serialized, type, env, overridden, recurse) => overridden("overridden", type, env, recurse))
                .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override2()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<string>((serialized, type, env, overridden, recurse) => overridden("overridden 1", type, env, recurse))
                .Override<string>((serialized, type, env, overridden, recurse) => overridden("overridden 2", type, env, recurse))
                .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<string>((serialized, type, env, overridden, recurse) => recurse.TryDeserialize(36, type, env))
                .TryDeserialize("serialized", typeof(int), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public void Recurse_wrong_type()
        {
            GremlinQueryFragmentDeserializer.Identity
                .Override<string>((serialized, type, env, overridden, recurse) => recurse.TryDeserialize(36, type, env))
                .Invoking(_ => _
                    .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty))
                .Should()
                .Throw<InvalidCastException>();
        }

        [Fact]
        public async Task Recurse_to_previous_override()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<int>((serialized, type, env, overridden, recurse) => overridden(37, type, env, recurse))
                .Override<string>((serialized, type, env, overridden, recurse) => recurse.TryDeserialize(36, type, env))
                .TryDeserialize("serialized", typeof(int), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse_to_later_override()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<string>((serialized, type, env, overridden, recurse) => recurse.TryDeserialize(36, type, env))
                .Override<int>((serialized, type, env, overridden, recurse) => overridden(37, type, env, recurse))
                .TryDeserialize("serialized", typeof(int), GremlinQueryEnvironment.Empty));
        }
    }
}
