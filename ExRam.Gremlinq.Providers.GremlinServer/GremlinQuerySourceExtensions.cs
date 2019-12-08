using System;
using System.Collections.Generic;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQuerySource UseGremlinServer(this IGremlinQuerySource source,
            Uri uri,
            GraphsonVersion graphsonVersion,
            string? username = null,
            string? password = null,
            string alias = "g",
            IReadOnlyDictionary<Type, IGraphSONSerializer>? additionalGraphsonSerializers = null,
            IReadOnlyDictionary<string, IGraphSONDeserializer>? additionalGraphsonDeserializers = null)
        {
            return source
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(opt => opt
                        .SetItem(GremlinQuerySerializer.WorkaroundTinkerpop2323, true)))
                .UseWebSocket(
                    uri,
                    graphsonVersion,
                    username,
                    password,
                    alias,
                    additionalGraphsonSerializers,
                    additionalGraphsonDeserializers);
        }
    }
}
