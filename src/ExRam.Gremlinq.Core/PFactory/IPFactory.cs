using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal interface IPFactory
    {
        P? TryGetP(ExpressionSemantics semantics, object? value, IGremlinQueryEnvironment environment);
    }
}
