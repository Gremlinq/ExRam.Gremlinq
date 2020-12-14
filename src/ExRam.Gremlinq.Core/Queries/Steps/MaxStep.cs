using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class MaxStep : Step
    {
        public static readonly MaxStep Local = new MaxStep(Scope.Local);
        public static readonly MaxStep Global = new MaxStep(Scope.Global);

        public MaxStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
