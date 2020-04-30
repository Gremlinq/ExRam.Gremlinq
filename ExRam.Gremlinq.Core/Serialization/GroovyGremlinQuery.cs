using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class GroovyGremlinQuery
    {
        public GroovyGremlinQuery(string script, Dictionary<string, object> bindings)
        {
            Script = script;
            Bindings = bindings;
        }

        public override string ToString()
        {
            return Script;
        }

        public string Script { get; }
        public Dictionary<string, object> Bindings { get; }
    }
}
