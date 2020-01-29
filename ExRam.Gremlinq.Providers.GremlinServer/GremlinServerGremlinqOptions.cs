using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.GremlinServer
{
    public static class GremlinServerGremlinqOptions
    {
        public static readonly GremlinqOption<bool> WorkaroundTinkerpop2112 = new GremlinqOption<bool>(false);
    }
}