using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Transformation;
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
                    .TryTransformTo<string>().From(42, GremlinQueryEnvironment.Empty))
                .Should()
                .Throw<InvalidOperationException>();
        }
    }
}
