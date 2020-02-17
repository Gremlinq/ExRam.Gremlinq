using System;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core
{
    public interface IWebSocketConfigurationBuilder
    {
        IWebSocketConfigurationBuilder At(Uri uri);

        IWebSocketConfigurationBuilder AuthenticateBy(string username, string password);

        IWebSocketConfigurationBuilder SetAlias(string alias);

        IWebSocketConfigurationBuilder SetGraphSONVersion(GraphsonVersion version);

        IWebSocketConfigurationBuilder AddGraphSONSerializer(Type type, IGraphSONSerializer serializer);

        IWebSocketConfigurationBuilder AddGraphSONDeserializer(string typename, IGraphSONDeserializer serializer);

        IGremlinQueryEnvironment Build();
    }
}
