using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class BytecodeGremlinQuery : ISerializedGremlinQuery
    {
        public BytecodeGremlinQuery(string queryId, Bytecode bytecode)
        {
            Id = queryId;
            Bytecode = bytecode;
        }

        public string Id { get; }

        public Bytecode Bytecode { get; }
    }
}
