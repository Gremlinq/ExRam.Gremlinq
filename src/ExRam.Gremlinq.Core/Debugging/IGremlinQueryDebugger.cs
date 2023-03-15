#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryDebugger
    {
        string Debug(Bytecode bytecode, IGremlinQueryEnvironment environment);
    }
}
