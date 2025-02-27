using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ToLowerStep : Step
    {
        public static readonly ToLowerStep Global = new(Scope.Global);

        private ToLowerStep(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}
