using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class GroovyGremlinQuery : ISerializedGremlinQuery
    {
        public GroovyGremlinQuery(string script, IReadOnlyDictionary<string, object> bindings) : this(Guid.NewGuid().ToString(), script, bindings)
        {
        }

        public GroovyGremlinQuery(string id, string script, IReadOnlyDictionary<string, object> bindings)
        {
            Id = id;
            Script = script;
            Bindings = bindings;
        }

        public override string ToString() => Script;

        public ISerializedGremlinQuery WithNewId() => new GroovyGremlinQuery(Script, Bindings);

        public string Script { get; }

        public string Id { get; }

        public IReadOnlyDictionary<string, object> Bindings { get; }
    }
}
