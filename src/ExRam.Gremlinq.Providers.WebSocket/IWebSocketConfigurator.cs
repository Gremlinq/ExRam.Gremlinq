using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketConfigurator : IGremlinQuerySourceTransformation
    {
        IWebSocketConfigurator At(Uri uri);

        IWebSocketConfigurator AuthenticateBy(string username, string password);

        IWebSocketConfigurator SetAlias(string alias);

        IWebSocketConfigurator ConfigureConnectionPool(Action<ConnectionPoolSettings> transformation);

        IWebSocketConfigurator ConfigureGremlinClient(Func<IGremlinClient, IGremlinClient> transformation);

        IWebSocketConfigurator SetSerializationFormat(SerializationFormat version);

        IWebSocketConfigurator AddGraphSONSerializer(Type type, IGraphSONSerializer serializer);
    }
}
