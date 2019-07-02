using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.Serialization
{
    public struct GroovySerializedGremlinQuery
    {
        public GroovySerializedGremlinQuery(string queryString, Dictionary<string, object> bindings)
        {
            QueryString = queryString;
            Bindings = bindings;
        }

        public override string ToString()
        {
            return QueryString;
        }

        public string QueryString { get; }
        public Dictionary<string, object> Bindings { get; }
    }
}
