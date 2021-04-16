using System;

namespace ExRam.Gremlinq.Providers.GremlinServer
{
    public static class GremlinServerConfiguratorExtensions
    {
        public static IGremlinServerConfigurator AtLocalhost(this IGremlinServerConfigurator configurator)
        {
            return configurator.At(new Uri("ws://localhost:8182"));
        }
    }
}
