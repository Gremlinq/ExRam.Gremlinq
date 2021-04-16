using System;

namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public static class JanusGraphConfiguratorExtensions
    {
        public static IJanusGraphConfigurator AtLocalhost(this IJanusGraphConfigurator configurator)
        {
            return configurator.At(new Uri("ws://localhost:8182"));
        }
    }
}
