using System;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core
{
    public interface IWebSocketQuerySourceBuilder
    {
        IWebSocketQuerySourceBuilder WithUri(Uri uri);

        IWebSocketQuerySourceBuilder WithGraphSONVersion(GraphsonVersion version);

        IWebSocketQuerySourceBuilder WithAuthentication(string username, string password);

        IWebSocketQuerySourceBuilder WithAlias(string alias);

        IWebSocketQuerySourceBuilder WithGraphSONSerializer(Type type, IGraphSONSerializer serializer);

        IWebSocketQuerySourceBuilder WithGraphSONDeserializer(string typename, IGraphSONDeserializer serializer);

        IGremlinQueryEnvironment Build();
    }
}
