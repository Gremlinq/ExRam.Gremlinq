using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.Serialization
{
    public struct SerializedGremlinQuery
    {
        public SerializedGremlinQuery(string queryString, Dictionary<string, object> bindings)
        {
            QueryString = queryString;
            Bindings = bindings;
        }

        public string QueryString { get; }
        public Dictionary<string, object> Bindings { get; }
    }
}
