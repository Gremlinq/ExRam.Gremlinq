using System;

namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfigurationBuilder
    {
        IJanusGraphConfigurationBuilderWithUri At(Uri uri);
    }
}
