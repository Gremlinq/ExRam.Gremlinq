using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class Graphson2StringSerializationFixture : GraphSonStringSerializationFixture
    {
        public Graphson2StringSerializationFixture() : base(new GraphSON2Writer())
        {
        }
    }
}
