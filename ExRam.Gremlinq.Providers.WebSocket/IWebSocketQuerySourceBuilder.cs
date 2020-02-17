using System;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core
{
    public interface IWebSocketQuerySourceBuilder
    {
        IWebSocketQuerySourceBuilder UseUri(Uri uri);

        IWebSocketQuerySourceBuilder UseGraphSONVersion(GraphsonVersion version);

        IWebSocketQuerySourceBuilder UseAuthentication(string username, string password);

        IWebSocketQuerySourceBuilder UseAlias(string alias);

        IWebSocketQuerySourceBuilder UseGraphSONSerializer(Type type, IGraphSONSerializer serializer);

        IWebSocketQuerySourceBuilder UseGraphSONDeserializer(string typename, IGraphSONDeserializer serializer);
    }
}
