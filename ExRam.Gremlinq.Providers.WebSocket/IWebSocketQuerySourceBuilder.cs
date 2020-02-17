using System;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core
{
    public interface IWebSocketQuerySourceBuilder
    {
        IWebSocketQuerySourceBuilder At(Uri uri);

        IWebSocketQuerySourceBuilder AuthenticateBy(string username, string password);

        IWebSocketQuerySourceBuilder SetAlias(string alias);

        IWebSocketQuerySourceBuilder SetGraphSONVersion(GraphsonVersion version);

        IWebSocketQuerySourceBuilder AddGraphSONSerializer(Type type, IGraphSONSerializer serializer);

        IWebSocketQuerySourceBuilder AddGraphSONDeserializer(string typename, IGraphSONDeserializer serializer);

        IGremlinQueryEnvironment Build();
    }
}
