using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Providers.Core;
using FluentAssertions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class DeserializerTests
    {
        [Fact]
        public async Task Incomplete_deserializer()
        {
            GremlinQuerySource.g
                .UseGremlinServer<Vertex, Edge>(_ => _
                    .AtLocalhost())
                .AsAdmin()
                .Environment.Deserializer
                .Invoking(_ => _
                    .TryTransformTo<string>().From(42, GremlinQueryEnvironment.Invalid))
                .Should()
                .Throw<InvalidOperationException>();
        }
    }
}
