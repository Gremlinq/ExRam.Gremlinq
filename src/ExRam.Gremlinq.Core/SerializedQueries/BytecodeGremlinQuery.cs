using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public readonly struct BytecodeGremlinQuery
    {
        private readonly Bytecode? _bytecode;

        public BytecodeGremlinQuery(Bytecode bytecode)
        {
            _bytecode = bytecode;
        }

        public Bytecode Bytecode => _bytecode ?? throw new InvalidOperationException();
    }
}
