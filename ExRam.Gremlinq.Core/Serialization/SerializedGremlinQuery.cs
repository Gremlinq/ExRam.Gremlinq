using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.Serialization
{
    public struct SerializedGremlinQuery
    {
        public SerializedGremlinQuery(string queryString, IDictionary<string, object> bindings)
        {
            QueryString = queryString;
            Bindings = bindings;
        }

        public string QueryString { get; }
        public IDictionary<string, object> Bindings { get; }
    }
}
