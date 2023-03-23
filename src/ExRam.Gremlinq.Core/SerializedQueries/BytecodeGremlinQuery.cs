using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public readonly struct BytecodeGremlinQuery
    {
        private readonly string? _id;
        private readonly Bytecode? _bytecode;

        public BytecodeGremlinQuery(Bytecode bytecode) : this(Guid.NewGuid().ToString(), bytecode)
        {
        }

        public BytecodeGremlinQuery(string queryId, Bytecode bytecode)
        {
            _id = queryId;
            _bytecode = bytecode;
        }

        public string Id => _id ?? throw new InvalidOperationException();

        public Bytecode Bytecode => _bytecode ?? throw new InvalidOperationException();
    }
}
