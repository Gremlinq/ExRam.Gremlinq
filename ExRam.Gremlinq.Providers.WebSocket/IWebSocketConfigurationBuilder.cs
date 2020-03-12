using System;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketConfigurationBuilder : IGremlinQueryEnvironmentBuilder
    {
        IWebSocketConfigurationBuilder At(Uri uri);

        IWebSocketConfigurationBuilder AuthenticateBy(string username, string password);

        IWebSocketConfigurationBuilder SetAlias(string alias);

        IWebSocketConfigurationBuilder ConfigureConnectionPool(Func<ConnectionPoolSettings> connectionPoolSettings);

        IWebSocketConfigurationBuilder SetGraphSONVersion(GraphsonVersion version);

        IWebSocketConfigurationBuilder AddGraphSONSerializer(Type type, IGraphSONSerializer serializer);

        IWebSocketConfigurationBuilder AddGraphSONDeserializer(string typename, IGraphSONDeserializer serializer);

        IWebSocketConfigurationBuilder ConfigureQueryLoggingOptions(Func<QueryLoggingOptions, QueryLoggingOptions> transformation);
    }
}
