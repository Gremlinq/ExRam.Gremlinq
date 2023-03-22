using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class BytecodeGremlinQuery
    {
        public BytecodeGremlinQuery(Bytecode bytecode) : this(Guid.NewGuid().ToString(), bytecode)
        {
        }

        public BytecodeGremlinQuery(string queryId, Bytecode bytecode)
        {
            Id = queryId;
            Bytecode = bytecode;
        }

        public BytecodeGremlinQuery WithNewId() => new (Bytecode);

        public string Id { get; }

        public Bytecode Bytecode { get; }
    }
}
