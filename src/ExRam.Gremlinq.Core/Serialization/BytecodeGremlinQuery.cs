using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class BytecodeGremlinQuery : ISerializedQuery
    {
        public BytecodeGremlinQuery(Bytecode bytecode)
        {
            Bytecode = bytecode;
        }

        public Bytecode Bytecode { get; }
    }
}
