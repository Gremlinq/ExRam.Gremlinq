using FluentAssertions;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class TypeTests
    {
        [Fact]
        public void ResponseMessagePayload_exists() => typeof(IGremlinqClientFactory).Assembly.DefinedTypes
                .Any(x => x.Name == "ResponseMessagePayload`1")
                .Should()
                .BeTrue();

        [Fact]
        public void ResponseMessageEnvelope_exists() => typeof(IGremlinqClientFactory).Assembly.DefinedTypes
                .Any(x => x.Name == "ResponseMessageEnvelope")
                .Should()
                .BeTrue();
    }
}
