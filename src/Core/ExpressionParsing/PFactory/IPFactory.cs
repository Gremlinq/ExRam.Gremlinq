using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    public interface IPFactory
    {
        P? TryGetP(ExpressionSemantics semantics, object? value, IGremlinQueryEnvironment environment);
    }
}
