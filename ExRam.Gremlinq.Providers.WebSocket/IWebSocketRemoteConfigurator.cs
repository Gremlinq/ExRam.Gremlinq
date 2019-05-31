using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketRemoteConfigurator
    {
        IWebSocketRemoteConfigurator WithClient(IGremlinClient client);

        IWebSocketRemoteConfigurator WithSerializerFactory(IGraphsonSerializerFactory serializer);

        IWebSocketRemoteConfigurator WithVisitor(IGremlinQueryElementVisitor visitor);

        IGremlinQueryExecutor Build();
    }
}
