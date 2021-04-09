using System;

namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfigurator
    {
        IJanusGraphConfiguratorWithUri At(Uri uri);
    }
}
