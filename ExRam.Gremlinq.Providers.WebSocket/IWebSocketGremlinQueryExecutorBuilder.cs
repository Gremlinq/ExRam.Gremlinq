using System;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketGremlinQueryExecutorBuilder : IGremlinQueryExecutorBuilder
    {
        IWebSocketGremlinQueryExecutorBuilder At(Uri uri);

        IWebSocketGremlinQueryExecutorBuilder AuthenticateBy(string username, string password);

        IWebSocketGremlinQueryExecutorBuilder SetAlias(string alias);

        IWebSocketGremlinQueryExecutorBuilder ConfigureConnectionPool(Action<ConnectionPoolSettings> transformation);

        IWebSocketGremlinQueryExecutorBuilder ConfigureGremlinClient(Func<IGremlinClient, IGremlinClient> transformation);

        IWebSocketGremlinQueryExecutorBuilder SetSerializationFormat(SerializationFormat version);

        IWebSocketGremlinQueryExecutorBuilder AddGraphSONSerializer(Type type, IGraphSONSerializer serializer);

        IWebSocketGremlinQueryExecutorBuilder AddGraphSONDeserializer(string typename, IGraphSONDeserializer serializer);
    }
}
