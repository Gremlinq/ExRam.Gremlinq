using FluentAssertions;

using Gremlin.Net.Process.Traversal;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class SerializerTest
    {
        // Gremlin.Net's P.Within puts its arguments into a List<object>, so a P with a collection that is
        // no IList, or with a plain enumerable, is one built by hand.
        [Fact]
        public void P_with_a_collection_value_serializes_its_items() => SerializedValueOf(new P("within", new Queue<int>([36, 37, 38])))
            .Should()
            .BeEquivalentTo(new object[] { 36, 37, 38 }, options => options.WithStrictOrdering());

        [Fact]
        public void P_with_an_enumerable_value_serializes_its_items() => SerializedValueOf(new P("within", Enumerable.Range(36, 3).Select(x => x)))
            .Should()
            .BeEquivalentTo(new object[] { 36, 37, 38 }, options => options.WithStrictOrdering());

        private static object? SerializedValueOf(P p)
        {
            var environment = g
                .AsAdmin()
                .Environment;

            return environment.Serializer
                .TransformTo<object>()
                .From(p, environment)
                .Should()
                .BeOfType<P>()
                .Subject
                .Value;
        }
    }
}
