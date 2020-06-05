using System;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketGremlinQueryEnvironmentBuilder : IGremlinQueryEnvironmentBuilder
    {
        IWebSocketGremlinQueryEnvironmentBuilder At(Uri uri);

        IWebSocketGremlinQueryEnvironmentBuilder AuthenticateBy(string username, string password);

        IWebSocketGremlinQueryEnvironmentBuilder SetAlias(string alias);

        IWebSocketGremlinQueryEnvironmentBuilder ConfigureConnectionPool(Action<ConnectionPoolSettings> transformation);

        IWebSocketGremlinQueryEnvironmentBuilder ConfigureGremlinClient(Func<IGremlinClient, IGremlinClient> transformation);

        IWebSocketGremlinQueryEnvironmentBuilder SetSerializationFormat(SerializationFormat version);

        IWebSocketGremlinQueryEnvironmentBuilder AddGraphSONSerializer(Type type, IGraphSONSerializer serializer);

        IWebSocketGremlinQueryEnvironmentBuilder AddGraphSONDeserializer(string typename, IGraphSONDeserializer serializer);
    }
}
