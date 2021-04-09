using System;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketConfigurator : IGremlinQueryEnvironmentTransformation
    {
        IWebSocketConfigurator At(Uri uri);

        IWebSocketConfigurator AuthenticateBy(string username, string password);

        IWebSocketConfigurator SetAlias(string alias);

        IWebSocketConfigurator ConfigureConnectionPool(Action<ConnectionPoolSettings> transformation);

        IWebSocketConfigurator ConfigureGremlinClient(Func<IGremlinClient, IGremlinClient> transformation);

        IWebSocketConfigurator SetSerializationFormat(SerializationFormat version);

        IWebSocketConfigurator AddGraphSONSerializer(Type type, IGraphSONSerializer serializer);

        IWebSocketConfigurator AddGraphSONDeserializer(string typename, IGraphSONDeserializer serializer);
    }
}
