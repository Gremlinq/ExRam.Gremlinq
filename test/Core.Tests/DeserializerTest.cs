using ExRam.Gremlinq.Core.Deserialization;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class DeserializerTest
    {
        [Fact]
        public async Task Empty()
        {
            Deserializer.Default
                .Invoking(_ => _
                    .TryTransformTo<string>().From(42, GremlinQueryEnvironment.Invalid))
                .Should()
                .Throw<InvalidOperationException>();
        }
    }
}
